using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    private PlayerMovement playerMovement;
    PlayerControls controls;
    public Animator animator;
    public GameObject bullet;
    public Transform bulletHole;
    public float force = 200;

    public void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        controls = new PlayerControls();
        controls.Enable();

        controls.Land.Throw.performed += ctx => Throw();
    }

    void Throw()
    {
        GameObject go = Instantiate(bullet, bulletHole.position, bullet.transform.rotation);
        if(GetComponent<PlayerMovement>().isFacingRight)
        {
            if(!playerMovement.isGrounded)
                animator.SetTrigger("jumpThrow");
            else
                animator.SetTrigger("throw");
            
            go.GetComponent<Rigidbody2D>().AddForce(Vector2.right * force);
        }
        
        else
        {
            if(!playerMovement.isGrounded)
                animator.SetTrigger("jumpThrow");
            else
                animator.SetTrigger("throw");

            go.GetComponent<Rigidbody2D>().SetRotation(90);
            go.GetComponent<Rigidbody2D>().AddForce(Vector2.left * force);
        }
    }
}
