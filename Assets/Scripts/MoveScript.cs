using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveScript : MonoBehaviour
{
    // used to retrieve the action map and action. Must have generated the associated C# class. InputActions != InputAction.
    private InputActions inputActions;
    private Rigidbody playerRigidBody; // forces are applied on the rigid body
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Transform cameraTransform;

    private Vector2 moveInput;
    private float moveSpeed = 2f;
    private float sprintSpeedMultiplier = 1f;
    private bool shiftLock = false;

    private void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        inputActions = new InputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        // event subscriptions
        inputActions.Player.Move.performed += Move;
        inputActions.Player.Move.canceled += Move;

        inputActions.Player.Sprint.performed += Sprint;
        inputActions.Player.Sprint.canceled += Sprint;

        inputActions.Player.ShiftLock.started += ShiftLock;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= Move;
        inputActions.Player.Move.canceled -= Move;

        inputActions.Player.Sprint.performed -= Sprint;
        inputActions.Player.Sprint.canceled -= Sprint;

        inputActions.Player.ShiftLock.started -= ShiftLock;

        inputActions.Player.Disable();
    }

    private void Sprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            sprintSpeedMultiplier = 2f;
        }
        if (context.canceled)
        {
            sprintSpeedMultiplier = 1f;
        }
    }

    private void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void ShiftLock(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            shiftLock = !shiftLock; // if true, it will become false, and vice-versa
        }
    }

    // continuous forces are not applied in Update() because the physics engine does not apply forces every frame,
    // but only once after a fixed amount of time (FixedUpdate's activation rate), so doing it in update would build-up
    // the force and apply it all at once
    private void FixedUpdate()
    {        
        if (shiftLock)
        {
            transform.rotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
            groundCheckPoint.rotation = transform.rotation;

        }
        else
        {
            groundCheckPoint.rotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
        }

        Vector3 camForward = groundCheckPoint.forward;
        Vector3 camRight = groundCheckPoint.right;

        Vector3 desiredDirection = moveInput.y * camForward + moveInput.x * camRight;

        if (desiredDirection != Vector3.zero && !shiftLock)
        {
            transform.rotation = Quaternion.LookRotation(desiredDirection);
        }

        Vector3 horizontalMovement = desiredDirection * moveSpeed * sprintSpeedMultiplier;
        playerRigidBody.linearVelocity = new Vector3(horizontalMovement.x, playerRigidBody.linearVelocity.y, horizontalMovement.z);
        
        /*
        Player is looking right -> camForward = (1, 0, 0), camRight = (0, 0, -1)
        He wants to go left (a is pressed) -> moveInput = (-1, 0)

        desiredDirection = 0 * (1, 0, 0) + (-1) * (0, 0, -1)
                         = (0, 0, 1)

        So the player will move in the direction of the Z axis (exactly what we wanted)
        */
    }
}