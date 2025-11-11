using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private EntityScriptableObject entityScriptableObject;

    [SerializeField]
    private InputActionReference moveAction;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private bool isGrounded;

    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private CapsuleCollider2D groundCheck;

    [SerializeField]
    private float startJumpHeight;

    [SerializeField]
    private bool reachedMaxJump;

    private PlayerStateManager playerStateManager;

    private Vector2 movement;

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

    private void CheckIfReachMaxJumpHeight()
    {
        if (playerStateManager.CurrentState == PlayerState.Jumping)
        {
            reachedMaxJump = rb.position.y - startJumpHeight >= entityScriptableObject.jumpHeight;
        }
        else
        {
            reachedMaxJump = false;
        }
    }

    private void CheckGrounded()
    {
        Vector2 center = groundCheck.bounds.center;
        Collider2D[] colliders = Physics2D.OverlapCapsuleAll(
            center,
            groundCheck.size,
            CapsuleDirection2D.Horizontal,
            0,
            groundMask
        );
        isGrounded = colliders.Length > 0;
    }

    private void Move()
    {
        Vector2 curPosition = rb.position;
        Vector2 change = new(entityScriptableObject.speed * Time.fixedDeltaTime, GetYMovement());
        if (!isGrounded)
        {
            movement.y = 1;
        }

        rb.MovePosition(curPosition + (change * movement));
    }

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

    private float GetYMovement()
    {
        if (
            playerStateManager.CurrentState == PlayerState.StartJump
            || playerStateManager.CurrentState == PlayerState.Jumping
        )
        {
            if (playerStateManager.CurrentState == PlayerState.StartJump)
            {
                startJumpHeight = rb.position.y;
            }
            return SolveKinematicsEquation(entityScriptableObject.jumpSpeed, 0);
        }
        else if (playerStateManager.CurrentState == PlayerState.Fall)
        {
            return GetGravityMovement();
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
            rb.linearVelocityY,
            entityScriptableObject.gravityForce * entityScriptableObject.gravityScale
        );
    }

    private float SolveKinematicsEquation(float velocity, float acceleration)
    {
        return (velocity * Time.fixedDeltaTime)
            + (0.5f * acceleration * Time.fixedDeltaTime * Time.fixedDeltaTime);
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
}
