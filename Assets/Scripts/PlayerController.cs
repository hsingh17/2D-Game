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

    private Vector2 movement;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        movement = moveAction.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        CheckGrounded();
        Move();
    }

    private void CheckGrounded()
    {
        // Change to raycast
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
        Vector2 change = new(entityScriptableObject.speed * Time.fixedDeltaTime, 0);

        if (isGrounded && movement.y != 0)
        {
            change.y = SolveKinematicsEquation(entityScriptableObject.jumpSpeed, 0);
        }
        else if (!isGrounded)
        {
            change.y = GetGravityMovement();
            movement.y = 1;
        }
        rb.MovePosition(curPosition + (change * movement));
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
}
