using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /*
    This scripts controls all the player movements => run, slide, jump, double jump
    */
    
    PlayerControls controls;
    public Animator animator;
    float direction = 0;  // Direction of the player is facing
    public float speed = 400;  // Running speed of the player
    public float jumpForce = 5;   // Jump force of the player
    public bool isGrounded;
    int numberOfJumps = 0;   // Number of jumps performed
    public Transform groundCheck;
    public LayerMask groundLayer;
    public bool isFacingRight = true;   // Bool to check the direction the player is facing
    public Rigidbody2D playerRB;

    public void Awake()
    {
        // Create new instance of PlayerControls
        controls = new PlayerControls();  
    }

    private void OnEnable()
    {
        // If "right arrow key"=> "left", "left arrow key"=>"right", "C"=>"slide", "Space bar"=>"jump" is performed
        controls.Enable();
        controls.Land.Move.performed += OnMovePerformed;
        controls.Land.Move.canceled += OnMoveCanceled;
        controls.Land.Jump.performed += OnJumpPerformed;
        controls.Land.Slide.performed += OnSlidePerformed;
    }

    private void OnDisable()
    {
        // If not performed
        controls.Land.Move.performed -= OnMovePerformed;
        controls.Land.Move.canceled -= OnMoveCanceled;
        controls.Land.Jump.performed -= OnJumpPerformed;
        controls.Land.Slide.performed -= OnSlidePerformed;
        controls.Disable();
    }

    private void OnMovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        direction = context.ReadValue<float>();  // If moving take the value
    }

    private void OnMoveCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        direction = 0;  // If moving is not performed setting moving direction to 0
    }

    private void OnJumpPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Jump(); // Perform jump
    }

    private void OnSlidePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Slide(); // Perform slide
    }

    // Fixed update when moving the player
    void FixedUpdate()
    {
        CheckGround(); // Check if player is grounded
        Move(); // Check if player is moving
        UpdateAnimation(); // Update animations accordingly
        FlipPlayer(); // Flip player if direction is changed
    }

    private void CheckGround()
    {
        if(groundCheck != null && groundLayer != 0)
        {
            // Checks if player is grounded using the overlap circle function(with radius 0.1)
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
            if(animator != null)
            {
                animator.SetBool("isGrounded", isGrounded);  // Using this idle and run animation states accordingly
            }
                
        } 
    }

    private void Move()
    {
        if(playerRB != null)
        {
            // Making the player move in the given direction
            playerRB.velocity = new Vector2(direction * speed * Time.fixedDeltaTime, playerRB.velocity.y);  

            // Play audio checking when running, checking whether player is a ninja or robot
            if(PlayerManager.isNinja)
            {
                AudioManagerPlayer.instance.Play("NinjaRun");
            }
            else
            {
                AudioManagerPlayer.instance.Play("RobotRun");
            }
        }
        
    }

    private void UpdateAnimation()
    {
        if(animator != null)
        {
            animator.SetFloat("speed", Mathf.Abs(direction));  // Tarnsfer between run and idle animation's of the player
        }
    }

    void FlipPlayer()
    {
        // Flip the player facing direction according to given input
        if(isFacingRight && direction < 0 || !isFacingRight && direction > 0)
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y); // Flipping happens here
        }
    }

    void Jump()
    {
        // This function checks whether player has jumped 1 time and allows to do another jump, so player can jump 2 times also.
        if(isGrounded)
        {
            numberOfJumps = 0;  // Set the jump count to 0  
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);  // Jump movement happens here
            numberOfJumps++;
            // Play jump sound effect for robot or ninja
            if(PlayerManager.isNinja)
            {
                AudioManagerPlayer.instance.Play("NinjaJump");
            }
            else
            {
                AudioManagerPlayer.instance.Play("RobotJump");
            }
        }
        else
        {
            if(numberOfJumps == 1)  // Checks whehther player has jumped 1 time and allows for the second
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
                numberOfJumps++;
                if(PlayerManager.isNinja)
            {
                AudioManagerPlayer.instance.Play("NinjaJump");
            }
            else
            {
                AudioManagerPlayer.instance.Play("RobotJump");
            }
            }
        }
    }

    void Slide()
    {
        if(animator != null)
        {
            animator.SetTrigger("slide"); // Play slide animation
        }
        
    }
}
