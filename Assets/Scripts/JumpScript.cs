using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpScript : MonoBehaviour
{
    private Rigidbody playerRigidBody;
    private CapsuleCollider playerCapsuleCollider;
    private Vector3 jumpForce = new Vector3(0f, 5f, 0f);
    [SerializeField] private Transform groundCheckPoint;
    private bool jumpPressed;

    private void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        playerCapsuleCollider = GetComponent<CapsuleCollider>();
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(groundCheckPoint.position, Vector3.down, 0.1f);
    }
    
    // HEAVY WIP; return the boolean used in PlayerAnimations script to launch the animation.
    public bool JumpPressed()
    {
        return true;
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            playerRigidBody.AddForce(jumpForce, ForceMode.Impulse);
            JumpPressed();
        }
    }
}