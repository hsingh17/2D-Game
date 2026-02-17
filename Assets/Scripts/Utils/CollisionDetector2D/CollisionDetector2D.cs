using System;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector2D : MonoBehaviour
{
    public bool DrawCollisions { get; set; }
    public HashSet<CollisionCast2D> Collisions { get; set; }
    private readonly Dictionary<string, CollisionDetect2D> collisionResults = new();

    public void AddCollisions(List<CollisionCast2D> collisions)
    {
        Collisions ??= new();

        foreach (CollisionCast2D cast in collisions)
        {
            if (Collisions.Contains(cast))
            {
                throw new Exception(
                    $"Duplicate collision with descriptor {cast.Descriptor} found!"
                );
            }

            Collisions.Add(cast);
        }
    }

    public void UpdateColliderForCollisions(Collider2D newCollider)
    {
        foreach (CollisionCast2D cast in Collisions)
        {
            cast.Collider = newCollider;
        }
    }

    public void CheckAllCollisions()
    {
        foreach (CollisionCast2D cast in Collisions)
        {
            RaycastHit2D hit = cast.CheckCollision();
            float hitDistance = CalculateHitDistance(hit, cast.Collider, cast.Direction);

            cast.Hit = hit;
            cast.HitDistance = hitDistance;

            Logger.Log($"{cast}");
        }
    }

    public CollisionDetect2D? GetCollisionResult(string collisionDescriptor)
    {
        if (
            collisionResults.TryGetValue(
                collisionDescriptor,
                out CollisionDetect2D collisionDetect2D
            )
        )
        {
            return collisionDetect2D;
        }

        return null;
    }

    public bool DidCollisionHit(string collisionDescriptor)
    {
        CollisionDetect2D? collisionDetect2D = GetCollisionResult(collisionDescriptor);
        return collisionDetect2D.HasValue && collisionDetect2D.Value.Hit;
    }

    private float CalculateHitDistance(RaycastHit2D hit, Collider2D collider, Vector2 direction)
    {
        if (!hit)
        {
            return 0;
        }

        Bounds colliderBounds = collider.bounds;
        Vector3 absDirVector = VectorUtils.ComponentAbsoluteValue(direction);
        float hitDotProduct = Vector3.Dot(hit.point, absDirVector);
        float centerDotProduct = Vector3.Dot(colliderBounds.center, absDirVector);
        float extentsDotProduct = Vector3.Dot(colliderBounds.extents, direction);

        return hitDotProduct - (centerDotProduct + extentsDotProduct);
    }

    private void OnDrawGizmos()
    {
        if (!DrawCollisions)
        {
            return;
        }

        foreach (CollisionCast2D collision in Collisions)
        {
            collision.DrawGizmos();
        }
    }
}
