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
        controls.Enable();

        controls.Land.Move.performed += ctx => direction = ctx.ReadValue<float>();
        controls.Land.Move.canceled += ctx => direction = 0;
        controls.Land.Jump.performed += ctx => Jump();
        controls.Land.Slide.performed += ctx => Slide();
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
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        animator.SetBool("isGrounded", isGrounded);
    }

    private void Move()
    {
        playerRB.velocity = new Vector2(direction * speed * Time.fixedDeltaTime, playerRB.velocity.y);
    }

    private void UpdateAnimation()
    {
        animator.SetFloat("speed", Mathf.Abs(direction));
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
        animator.SetTrigger("slide");
    }
}
