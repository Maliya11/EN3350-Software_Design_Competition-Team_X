using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerControls controls;
    public Animator animator;
    float direction = 0;
    public float speed = 400;
    public float jumpForce = 5;
    public bool isGrounded;
    int numberOfJumps = 0;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public bool isFacingRight = true;
    public Rigidbody2D playerRB;


    public void Awake()
    {
        controls = new PlayerControls();
    }

        private void OnEnable()
    {
        controls.Enable();
        controls.Land.Move.performed += OnMovePerformed;
        controls.Land.Move.canceled += OnMoveCanceled;
        controls.Land.Jump.performed += OnJumpPerformed;
        controls.Land.Slide.performed += OnSlidePerformed;
    }

    private void OnDisable()
    {
        controls.Land.Move.performed -= OnMovePerformed;
        controls.Land.Move.canceled -= OnMoveCanceled;
        controls.Land.Jump.performed -= OnJumpPerformed;
        controls.Land.Slide.performed -= OnSlidePerformed;
        controls.Disable();
    }

    private void OnMovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        direction = context.ReadValue<float>();
    }

    private void OnMoveCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        direction = 0;
    }

    private void OnJumpPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Jump();
    }

    private void OnSlidePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Slide();
    }


    
    //fixed update when moving the player
    void FixedUpdate()
    {
        CheckGround();
        Move();
        UpdateAnimation();
        FlipPlayer();
    }

    private void CheckGround()
    {
        if(groundCheck != null && groundLayer != 0)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
            if(animator != null)
            {
                animator.SetBool("isGrounded", isGrounded);
            }
                
        }
        
    }

    private void Move()
    {
        if(playerRB != null)
        {
            playerRB.velocity = new Vector2(direction * speed * Time.fixedDeltaTime, playerRB.velocity.y);
        }
        
    }

    private void UpdateAnimation()
    {
        if(animator != null)
        {
            animator.SetFloat("speed", Mathf.Abs(direction));
        }
        
    }

    void FlipPlayer()
    {
        if(isFacingRight && direction < 0 || !isFacingRight && direction > 0)
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }
    }

    void Jump()
    {
        if(isGrounded)
        {
            numberOfJumps = 0;
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
            numberOfJumps++;
        }
        else
        {
            if(numberOfJumps == 1)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
                numberOfJumps++;
            }
        }
    }

    void Slide()
    {
        if(animator != null)
        {
            animator.SetTrigger("slide");
        }
        
    }
}
