using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Class Variables

    #region Serializeable Fields

    [SerializeField]
    private EntityScriptableObject entityScriptableObject;

    [SerializeField]
    private InputActionReference moveAction;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private Collider2D standingCollider;

    [SerializeField]
    private Collider2D crouchedCollider;

    [SerializeField]
    private bool drawCollisions;

    [SerializeReference, SubclassSelector]
    private List<CollisionCast2D> collisions;

    #endregion

    #region Private Variables

    private Collider2D currentCollider;
    private PlayerAnimationStateManager playerAnimationStateManager;
    private Vector2 movement;
    private float startJumpY;
    private CollisionDetector2D collisionDetector2D;

    #endregion

    #endregion

    #region Functions

    #region MonoBehavior Functions

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        playerAnimationStateManager = gameObject.GetComponent<PlayerAnimationStateManager>();

        currentCollider = standingCollider;

        collisionDetector2D = gameObject.AddComponent<CollisionDetector2D>();
        collisionDetector2D.DrawCollisions = drawCollisions;
        collisionDetector2D.AddCollisions(collisions);
    }

    private void Update()
    {
        ReadMovement();
    }

    private void FixedUpdate()
    {
        CheckCollisions();
        UpdatePlayerState();
        DoAction();
    }

    #endregion

    #region Movement Related Functions

    private void ReadMovement()
    {
        movement = moveAction.action.ReadValue<Vector2>();
        // Y movement can only happen when on the ground (e.g: jumping and crouching)
        movement.y = IsGrounded() ? movement.y : 0;
    }

    private void CheckCollisions()
    {
        collisionDetector2D.CheckAllCollisions();
    }

    private void DoAction()
    {
        Crouch();
        Move();
    }

    private void Crouch()
    {
        if (
            playerAnimationStateManager.CurrentState == PlayerAnimationState.Crouch
            && currentCollider != crouchedCollider
        ) // Crouching and need to change collider to crouched
        {
            crouchedCollider.enabled = true;
            currentCollider = crouchedCollider;
            standingCollider.enabled = false;
        }
        else if (
            playerAnimationStateManager.CurrentState != PlayerAnimationState.Crouch
            && currentCollider != standingCollider
        ) // Not crouching and need to revert collider to standing
        {
            standingCollider.enabled = true;
            currentCollider = standingCollider;
            crouchedCollider.enabled = false;
        }

        collisionDetector2D.UpdateColliderForCollisions(currentCollider);
    }

    private void Move()
    {
        PlayerAnimationState curState = playerAnimationStateManager.CurrentState;

        // No moving during crouch
        if (curState == PlayerAnimationState.Crouch)
        {
            return;
        }

        // No moving horizontally if we detected a collision with ground
        if (
            (movement.x == -1 && !CanMoveHorizontal(Vector2.left))
            || (movement.x == 1 && !CanMoveHorizontal(Vector2.right))
        )
        {
            movement.x = 0;
        }

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
            playerAnimationStateManager.CurrentState == PlayerAnimationState.StartJump
            || playerAnimationStateManager.CurrentState == PlayerAnimationState.Jumping
        )
        {
            if (playerAnimationStateManager.CurrentState == PlayerAnimationState.StartJump)
            {
                startJumpY = rb.position.y;
            }
            return SolveKinematicsEquation(entityScriptableObject.jumpSpeed, 0);
        }
        else if (playerAnimationStateManager.CurrentState == PlayerAnimationState.Fall)
        {
            return GetGravityMovement();
        }
        else
        {
            return GetSnapToGroundDistance();
        }
    }

    private float GetGravityMovement()
    {
        if (IsGrounded())
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
        if (IsFalling())
        {
            playerAnimationStateManager.CurrentState = PlayerAnimationState.Fall;
        }
        else if (IsIdling())
        {
            playerAnimationStateManager.CurrentState = PlayerAnimationState.Idle;
        }
        else if (IsCrouching())
        {
            playerAnimationStateManager.CurrentState = PlayerAnimationState.Crouch;
        }
        else if (IsStartingJump())
        {
            playerAnimationStateManager.CurrentState = PlayerAnimationState.StartJump;
        }
        else if (IsJumping())
        {
            playerAnimationStateManager.CurrentState = PlayerAnimationState.Jumping;
        }
        else if (IsMoving())
        {
            playerAnimationStateManager.CurrentState =
                movement.x > 0 ? PlayerAnimationState.MoveRight : PlayerAnimationState.MoveLeft;
        }
    }

    private bool IsFalling()
    {
        bool notGroundedAndNotJumping =
            !IsGrounded()
            && playerAnimationStateManager.CurrentState != PlayerAnimationState.StartJump
            && playerAnimationStateManager.CurrentState != PlayerAnimationState.Jumping;

        return notGroundedAndNotJumping || ReachedMaxJump();
    }

    private bool IsIdling()
    {
        return IsGrounded() && movement == Vector2.zero;
    }

    private bool IsMoving()
    {
        return movement.y == 0 && movement.x != 0;
    }

    private bool IsStartingJump()
    {
        return IsGrounded() && movement.y == 1;
    }

    private bool IsJumping()
    {
        return !IsGrounded()
            && !ReachedMaxJump()
            && (
                playerAnimationStateManager.CurrentState == PlayerAnimationState.StartJump
                || playerAnimationStateManager.CurrentState == PlayerAnimationState.Jumping
            );
    }

    private bool IsCrouching()
    {
        return IsGrounded() && movement.y == -1;
    }

    private bool ReachedMaxJump()
    {
        if (playerAnimationStateManager.CurrentState == PlayerAnimationState.Jumping)
        {
            return rb.position.y - startJumpY >= entityScriptableObject.jumpHeight;
        }
        return false;
    }

    private bool IsGrounded()
    {
        return collisionDetector2D.DidCollisionHit("GroundCheck");
    }

    private float GetSnapToGroundDistance()
    {
        CollisionDetect2D? detect = collisionDetector2D.GetCollisionResult("GroundCheck");
        if (detect.HasValue)
        {
            return detect.Value.HitDistance;
        }
        else
        {
            return 0f;
        }
    }

    private bool CanMoveHorizontal(Vector2 dir)
    {
        return collisionDetector2D.DidCollisionHit("RightCheck")
            || collisionDetector2D.DidCollisionHit("LeftCheck");
    }

    #endregion

    #endregion
}
