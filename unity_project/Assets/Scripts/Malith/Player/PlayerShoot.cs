using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    private PlayerMovement playerMovement;
    PlayerControls controls;
    public Animator animator;

    public void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        controls = new PlayerControls();
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
