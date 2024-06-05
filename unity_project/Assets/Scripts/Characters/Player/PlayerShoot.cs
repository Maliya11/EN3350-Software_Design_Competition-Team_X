using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    /*
    This script is used to control the shooting of the player and identify to what enemy the attack is performed
    */

    private PlayerMovement playerMovement;
    PlayerControls controls;
    public Animator animator;
    public GameObject bullet;
    public Transform bulletHole;
    public float force = 200;   // Force of the bullet

    public void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();  // Get PlayerMovement component
        controls = new PlayerControls();  // Making PlayerControls instance
    }

    private void OnEnable()
    {
        // If shooting is performed
        controls.Enable();
        controls.Land.Throw.performed += OnThrowPerformed;
    }

    private void OnDisable()
    {
        // If attack is not performed
        controls.Land.Throw.performed -= OnThrowPerformed;
        controls.Disable();
    }

    private void OnThrowPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Throw();

        // Play the shoot sound effect after checking the player is a robot or a ninja
        if(PlayerManager.isNinja)
        {
            AudioManagerPlayer.instance.Play("NinjaThrow");
        }
        else
        {
            AudioManagerPlayer.instance.Play("RobotShoot");
        }
    }

    void Throw()
    {
        // Check if bulletHole and other necessary components are not null
        if (bulletHole == null || bullet == null || animator == null || playerMovement == null)
        {
            Debug.LogWarning("Necessary component missing for Throw action.");
            return;
        }

        // Instantiate the bullet at the bulletHole position
        GameObject go = Instantiate(bullet, bulletHole.position, bullet.transform.rotation);  
        
        if(GetComponent<PlayerMovement>().isFacingRight)
        {
            // Change the shoot/throw animation by checking the player is grounded or not
            if(!playerMovement.isGrounded)
                animator.SetTrigger("jumpThrow");
            else
                animator.SetTrigger("throw");
            
            // If facing right throw to the right
            go.GetComponent<Rigidbody2D>().AddForce(Vector2.right * force);  
        }
        
        else
        {
            // Change the attack animation by checking the player is grounded or not
            if(!playerMovement.isGrounded)
                animator.SetTrigger("jumpThrow");
            else
                animator.SetTrigger("throw");

            // Rotate the bullet to face the left direction
            go.GetComponent<Rigidbody2D>().SetRotation(90);

            // If facing left throw to the left
            go.GetComponent<Rigidbody2D>().AddForce(Vector2.left * force);  
        }
    }
}
