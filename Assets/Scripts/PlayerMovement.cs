using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private AudioClip footstepClip; 
    [SerializeField] private float stepInterval = 0.4f; 
private float stepTimer;
    public float moveSpeed = 5f;

    public static bool CanMove = true;

    private Rigidbody2D rb;
    private float moveInput;
    private bool facingRight = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!CanMove)
        {
            moveInput = 0f;
            return;
        }

        moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput > 0 && !facingRight) Flip();
        else if (moveInput < 0 && facingRight) Flip();

        if (moveInput != 0) 
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0)
            {
                if (AudioManager.Instance != null && footstepClip != null)
                {
                    AudioManager.Instance.PlaySFX(footstepClip, 0.7f);
                }
                stepTimer = stepInterval; 
            }
        }
        else
        {
            stepTimer = 0; 
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }
}