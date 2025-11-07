using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private readonly int run = Animator.StringToHash("Run");
    private readonly int idle = Animator.StringToHash("Idle");
    private readonly int jump = Animator.StringToHash("Jump");
    private readonly int fall = Animator.StringToHash("Fall");

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        PlayerController.onPlayerMove += AnimateOnPlayerStateChange;
    }

    private void AnimateOnPlayerStateChange(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Idle:
                animator.CrossFade(idle, 0, 0);
                break;
            case PlayerState.MoveLeft:
            case PlayerState.MoveRight:
                animator.CrossFade(run, 0, 0);
                FlipSprite(state);
                break;
            case PlayerState.Jump:
                animator.CrossFade(jump, 0, 0);
                break;
            case PlayerState.Fall:
                animator.CrossFade(fall, 0, 0);
                break;
        }
    }

    private void FlipSprite(PlayerState state)
    {
        bool movingRight = state == PlayerState.MoveRight;
        // Player movement direction does not align with the direction of sprite
        if (!spriteRenderer.flipX && !movingRight || spriteRenderer.flipX && movingRight)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
}
