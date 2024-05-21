using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerMovement playerMovement;
    PlayerControls controls;
    public Animator animator;

    public void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        controls = new PlayerControls();
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
