using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    #region Class Variables

    #region Animation Hashes

    private readonly int run = Animator.StringToHash("Run");
    private readonly int idle = Animator.StringToHash("Idle");
    private readonly int jump = Animator.StringToHash("Jump");
    private readonly int fall = Animator.StringToHash("Fall");
    private readonly int crouch = Animator.StringToHash("Crouch");

    #endregion

    #region Private Variables

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    #endregion

    #endregion

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        PlayerAnimationStateManager.onStateUpdate += AnimateOnPlayerStateChange;
    }

    private void OnDisable()
    {
        PlayerAnimationStateManager.onStateUpdate -= AnimateOnPlayerStateChange;
    }

    private void AnimateOnPlayerStateChange(PlayerAnimationState state)
    {
        switch (state)
        {
            case PlayerAnimationState.Idle:
                animator.CrossFade(idle, 0, 0);
                break;
            case PlayerAnimationState.MoveLeft:
            case PlayerAnimationState.MoveRight:
                animator.CrossFade(run, 0, 0);
                FlipSprite(state);
                break;
            case PlayerAnimationState.Jumping:
                animator.CrossFade(jump, 0, 0);
                break;
            case PlayerAnimationState.Fall:
                animator.CrossFade(fall, 0, 0);
                break;
            case PlayerAnimationState.Crouch:
                animator.CrossFade(crouch, 0, 0);
                break;
        }
    }

    private void FlipSprite(PlayerAnimationState state)
    {
        bool movingRight = state == PlayerAnimationState.MoveRight;
        // Player movement direction does not align with the direction of sprite
        if (!spriteRenderer.flipX && !movingRight || spriteRenderer.flipX && movingRight)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
}
