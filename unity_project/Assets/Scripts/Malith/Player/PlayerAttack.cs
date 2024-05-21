using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerControls controls;
    public Animator animator;
    private PlayerMovement playerMovement;

    void Awake()
    {
        controls = new PlayerControls();
        playerMovement = GetComponent<PlayerMovement>();
        controls.Enable();
        controls.Land.Attack.performed += ctx => Melee();
    }

    void Melee()
    {
        if(!playerMovement.isGrounded)
            animator.SetTrigger("jumpAttack");

        else
            animator.SetTrigger("attack");
    }
     
}
