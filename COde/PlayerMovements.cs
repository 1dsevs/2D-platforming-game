using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
    public Rigidbody2D body;
    public float moveSpeed = 5f;
    public float jumpForce = 6f;
    public bool isGrounded;
    
    private bool inWater = false;
    private Animator animator;
    private bool facingRight = true;
    private int coord_Y0 = 0;
    private int coord_Y1 = 0;
    private int coord_Y2 = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float x = 0f;

        // Movement left & right
        if (Keyboard.current.leftArrowKey.isPressed) x = -1f;
        if (Keyboard.current.rightArrowKey.isPressed) x = 1f;

        // Flip sprite
        if (x > 0 && !facingRight) Flip();
        if (x < 0 && facingRight) Flip();

        if (inWater)
        {
            // Reset gravity + stop sinking
            body.gravityScale = 0f;

            // Allow swimming up & down
            float y = 0f;
            if (Keyboard.current.upArrowKey.isPressed) y = 1f;
            if (Keyboard.current.downArrowKey.isPressed) y = -1f;

            body.linearVelocity = new Vector2(x * moveSpeed, y * moveSpeed);

            animator.Play("Swim");
        }
        else
        {
            // Normal gravity
            body.gravityScale = 3f;

            // Normal ground movement
            body.linearVelocity = new Vector2(x * moveSpeed, body.linearVelocity.y);

            if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
            {
                body.linearVelocity = new Vector2(body.linearVelocity.x, body.linearVelocity.y);
                body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isGrounded = false;
            }

            coord_Y2 = coord_Y1 ;            
            coord_Y1 = coord_Y0 ;
            coord_Y0 = body.linearVelocity.y ;

            if (coord_Y0 == coord_Y1 && coord_Y1 == coord_Y2){ isGrounded = true };

            animator.Play("Idle");
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!inWater && collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            inWater = true;
            body.linearVelocity = Vector2.zero;   // stop downward momentum
            body.gravityScale = 0f;        // remove gravity
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            inWater = false;
            body.gravityScale = 3f;        // restore gravity
        }
    }
    
}
