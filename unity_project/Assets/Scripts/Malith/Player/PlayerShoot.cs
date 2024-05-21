using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    PlayerControls controls;
    public Animator animator;
    private PlayerMovement playerMovement;

    void Awake()
    {
        controls = new PlayerControls();
        playerMovement = GetComponent<PlayerMovement>();
        controls.Enable();
        controls.Land.Throw.performed += ctx => Throw();
    }

    void Throw()
    {
        if(!playerMovement.isGrounded)
            animator.SetTrigger("jumpThrow");

        else
            animator.SetTrigger("throw");
    }
}
