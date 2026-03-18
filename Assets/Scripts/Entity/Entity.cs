using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity<T> : MonoBehaviour
    where T : Enum
{
    [SerializeField]
    protected Rigidbody2D rb;

    [SerializeField]
    protected Collider2D spriteCollider;

    [SerializeField]
    protected EntityScriptableObject scriptableObject;

    [SerializeField]
    protected bool drawCollisions;

    [SerializeReference, SubclassSelector]
    protected List<CollisionCast2D> collisions;

    protected CollisionDetector2D collisionDetector2D;

    protected abstract StateManager<T> StateManager { get; }
    protected abstract void UpdateState();
    protected abstract void DoAction();

    protected virtual void Awake()
    {
        collisionDetector2D = gameObject.AddComponent<CollisionDetector2D>();
        collisionDetector2D.DrawCollisions = drawCollisions;
        collisionDetector2D.AddCollisions(collisions);
    }

    protected virtual void CheckCollisions()
    {
        collisionDetector2D.CheckAllCollisions();
    }

    protected void FixedUpdate()
    {
        CheckCollisions();
        UpdateState();
        DoAction();
    }

    protected float GetGravityMovement()
    {
        if (IsGrounded())
        {
            return 0;
        }

        return SolveKinematicsEquation(
            0,
            scriptableObject.gravityForce * scriptableObject.gravityScale
        );
    }

    protected float SolveKinematicsEquation(float velocity, float acceleration)
    {
        return (velocity * Time.fixedDeltaTime)
            + (0.5f * acceleration * Time.fixedDeltaTime * Time.fixedDeltaTime);
    }

    protected bool IsGrounded()
    {
        return collisionDetector2D.DidCollisionHit("GroundCheck");
    }

    protected float GetSnapToGroundDistance()
    {
        CollisionCast2D cast = collisionDetector2D.GetCollisionResult("GroundCheck");
        return cast.HitDistance;
    }

    protected bool CanMoveHorizontal(Vector2 dir)
    {
        bool collisionDetected;

        if (dir == Vector2.left)
        {
            collisionDetected = collisionDetector2D.DidCollisionHit("LeftCheck");
        }
        else if (dir == Vector2.right)
        {
            collisionDetected = collisionDetector2D.DidCollisionHit("RightCheck");
        }
        else
        {
            throw new ArgumentException(
                $"Called CanMoveHorizontal with non-horizontal vector: {dir}"
            );
        }

        Logger.Log($"Direction: {dir} Can Move: {!collisionDetected}");

        return !collisionDetected;
    }
}
