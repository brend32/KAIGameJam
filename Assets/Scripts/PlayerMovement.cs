using Spine.Unity;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public SkeletonAnimation SkeletonAnimation;
    [SpineAnimation]
    public string IdleAnimationName = "idle";

    [SpineAnimation]
    public string WalkAnimationName = "walk";
    
    [SerializeField] private AudioClip footstepClip; 
    [SerializeField] private float stepInterval = 0.4f; 
    private float stepTimer;
    public float moveSpeed = 5f;

    public static bool CanMove = true;
    public string CurrentAnimationName;

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
            SetAnimation(WalkAnimationName, true);
            
            
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
            SetAnimation(IdleAnimationName, true);
 
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
    
    public void SetAnimation(string animationName, bool loop)
    {
        if (CurrentAnimationName == animationName) return;

        SkeletonAnimation.AnimationState.SetAnimation(0, animationName, loop);
        CurrentAnimationName = animationName;
    }
    
}