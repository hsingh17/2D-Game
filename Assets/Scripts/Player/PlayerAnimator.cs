using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = true;

        int run = Animator.StringToHash("Run");
        animator.CrossFade(run, 0, 0);
    }

    private void Update() { }

    private void FlipSprite()
    {
        // if (!spriteRenderer.flipX && movement.x < 0 || spriteRenderer.flipX && movement.x > 0)
        // {
        //     // Player movement direction does not align with the direction of sprite
        //     spriteRenderer.flipX = !spriteRenderer.flipX;
        // }
    }
}
