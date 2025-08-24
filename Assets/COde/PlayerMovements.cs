using UnityEngine;
using UnityEngine.InputSystem;
using Death;

public class PlayerMovements : MonoBehaviour
{
    public Rigidbody2D body;
    public float moveSpeed = 5f;
    public float jumpForce = 6f;
    public float swimSpeed = 3f;
    public bool inWater = false;
    public int timer = 0;

    private bool isGrounded;
    private Animator animator;
    private bool facingRight = true;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float x = 0f;
        float y = 0f;

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

            // Swimming up & down
            if (Keyboard.current.upArrowKey.isPressed) y = 1f;
            if (Keyboard.current.downArrowKey.isPressed) y = -1f;

            animator.Play("Walk-Swim");

            // Start the timer
            timer++;

            // Swim
            SwimMovement(x, y);

            animator.Play("Swim");
        }
        else
        {
            // Normal gravity
            body.gravityScale = 3f;

            // Reset the timer
            timer = 0;

            // Normal ground movement
            body.linearVelocity = new Vector2(x * moveSpeed, body.linearVelocity.y);

            if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
            {
                body.linearVelocity = new Vector2(body.linearVelocity.x, 0f); // reset Y before jump
                body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isGrounded = false;
            }

            animator.Play("Idle");
        }
    }

    private void SwimMovement(float x, float y)
    {
        // Swim movement
        body.linearVelocity = new Vector2(x * swimSpeed, y * swimSpeed);
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