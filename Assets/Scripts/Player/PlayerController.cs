using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Class Variables

    #region SerializeableFields

    [SerializeField]
    private EntityScriptableObject entityScriptableObject;

    [SerializeField]
    private InputActionReference moveAction;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private Collider2D collider2d;

    [SerializeField]
    private bool isGrounded;

    [SerializeField]
    private ContactFilter2D groundFilter;

    [SerializeField]
    private BoxCastProperties boxCastProperties;

    #endregion

    #region Private Variables

    private PlayerStateManager playerStateManager;
    private Vector2 movement;
    private float startJumpY;
    private bool reachedMaxJump;
    private float displacementToGround;

    #endregion

    #endregion

    #region Functions

    #region MonoBehavior Functions

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        playerStateManager = gameObject.GetComponent<PlayerStateManager>();
    }

    private void Update()
    {
        movement = moveAction.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        CheckIfReachMaxJumpHeight();
        CheckGrounded();
        UpdatePlayerState();
        Move();
    }

    #endregion

    #region Movement Related Functions

    private void CheckIfReachMaxJumpHeight()
    {
        if (playerStateManager.CurrentState == PlayerState.Jumping)
        {
            reachedMaxJump = rb.position.y - startJumpY >= entityScriptableObject.jumpHeight;
        }
        else
        {
            reachedMaxJump = false;
        }
    }

    private void CheckGrounded()
    {
        Bounds colliderBounds = collider2d.bounds;
        Vector3 colliderCenter = colliderBounds.center;
        Vector3 colliderExtents = colliderBounds.extents;
        Vector3 colliderSize = colliderBounds.size;

        RaycastHit2D hit = Physics2D.BoxCast(
            boxCastProperties.CalculateBoxCastOrigin(colliderCenter, colliderExtents),
            boxCastProperties.CalculateBoxCastSize(colliderSize),
            0f,
            Vector2.down,
            boxCastProperties.BoxCastDistance,
            groundMask
        );

        if (hit)
        {
            // Only save displacement if we just hit ground from a fall
            if (!isGrounded)
            {
                displacementToGround = hit.point.y - (colliderCenter.y - colliderExtents.y);
            }
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void Move()
    {
        movement.y = 1;

        Vector2 curPosition = rb.position;
        Vector2 change = new(entityScriptableObject.speed * Time.fixedDeltaTime, GetYMovement());
        Vector2 nextPosition = curPosition + (change * movement);

        rb.MovePosition(nextPosition);

        // Reset movement vector, noticed that if I didn't do this,
        // it would carry over the movement.y = 1 to the next loop
        movement = Vector2.zero;
    }

    private float GetYMovement()
    {
        if (
            playerStateManager.CurrentState == PlayerState.StartJump
            || playerStateManager.CurrentState == PlayerState.Jumping
        )
        {
            if (playerStateManager.CurrentState == PlayerState.StartJump)
            {
                startJumpY = rb.position.y;
            }
            return SolveKinematicsEquation(entityScriptableObject.jumpSpeed, 0);
        }
        else if (playerStateManager.CurrentState == PlayerState.Fall)
        {
            return GetGravityMovement();
        }
        else if (displacementToGround != 0)
        {
            float ret = displacementToGround;
            displacementToGround = 0;
            return ret;
        }
        else
        {
            return 0;
        }
    }

    private float GetGravityMovement()
    {
        if (isGrounded)
        {
            return 0;
        }

        return SolveKinematicsEquation(
            0,
            entityScriptableObject.gravityForce * entityScriptableObject.gravityScale
        );
    }

    private float SolveKinematicsEquation(float velocity, float acceleration)
    {
        return (velocity * Time.fixedDeltaTime)
            + (0.5f * acceleration * Time.fixedDeltaTime * Time.fixedDeltaTime);
    }

    #endregion

    #region Player State Update Functions

    private void UpdatePlayerState()
    {
        if (IsFall())
        {
            playerStateManager.CurrentState = PlayerState.Fall;
        }
        else if (IsIdle())
        {
            playerStateManager.CurrentState = PlayerState.Idle;
        }
        else if (IsMoving())
        {
            playerStateManager.CurrentState =
                movement.x > 0 ? PlayerState.MoveRight : PlayerState.MoveLeft;
        }
        else if (IsStartJump())
        {
            playerStateManager.CurrentState = PlayerState.StartJump;
        }
        else if (IsJumping())
        {
            playerStateManager.CurrentState = PlayerState.Jumping;
        }
    }

    private bool IsFall()
    {
        bool notGroundedAndNotJumping =
            !isGrounded
            && playerStateManager.CurrentState != PlayerState.StartJump
            && playerStateManager.CurrentState != PlayerState.Jumping;

        return notGroundedAndNotJumping || reachedMaxJump;
    }

    private bool IsIdle()
    {
        return isGrounded && movement == Vector2.zero;
    }

    private bool IsMoving()
    {
        return movement.y == 0 && movement.x != 0;
    }

    private bool IsStartJump()
    {
        return isGrounded && movement.y > 0;
    }

    private bool IsJumping()
    {
        return !isGrounded
            && !reachedMaxJump
            && playerStateManager.CurrentState == PlayerState.StartJump;
    }

    #endregion

    #endregion
}
