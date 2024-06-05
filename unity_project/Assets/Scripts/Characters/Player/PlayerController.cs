/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    Vector2 moveInput;
    private Vector3 lastSafePosition;

    // Property to check if the player is moving
    public bool isMoving { get; private set;}

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize last safe position to the starting position
        lastSafePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * walkSpeed, rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>());
        moveInput = context.ReadValue<Vector2>();

        isMoving = moveInput != Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            // Update last safe position when player touches the ground
            lastSafePosition = transform.position;
            Debug.Log("Last safe position updated to: " + lastSafePosition);
        }
    }

    public void Respawn()
    {
        transform.position = lastSafePosition;
    }
}
 */