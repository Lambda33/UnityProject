using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    JumpScript jumpScript;
    Animator playerAnimator;

    void Start()
    {
        jumpScript = GetComponent<JumpScript>();
        playerAnimator = GetComponent<Animator>();
    }
    // Jumping animation

    
}
