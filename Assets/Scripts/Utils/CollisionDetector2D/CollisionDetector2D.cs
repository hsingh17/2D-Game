using System;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector2D : MonoBehaviour
{
    public bool DrawCollisions { get; set; }
    private readonly Dictionary<string, CollisionCast2D> collisions = new();

    public void AddCollisions(List<CollisionCast2D> collisionList)
    {
        foreach (CollisionCast2D cast in collisionList)
        {
            if (collisions.ContainsKey(cast.Descriptor))
            {
                throw new Exception(
                    $"Duplicate collision with descriptor {cast.Descriptor} found!"
                );
            }

            collisions.Add(cast.Descriptor, cast);
        }
    }

    public void UpdateColliderForCollisions(Collider2D newCollider)
    {
        foreach (CollisionCast2D cast in collisions.Values)
        {
            cast.Collider = newCollider;
        }
    }

    public void CheckAllCollisions()
    {
        foreach (CollisionCast2D cast in collisions.Values)
        {
            RaycastHit2D hit = cast.CheckCollision();
            cast.Hit = hit;
            cast.HitDistance = CalculateHitDistance(hit, cast.Collider, cast.Direction);
            Logger.Log($"{cast}");
        }
    }

    public CollisionCast2D GetCollisionResult(string collisionDescriptor)
    {
        if (collisions.TryGetValue(collisionDescriptor, out CollisionCast2D collisionDetect2D))
        {
            return collisionDetect2D;
        }

        return null;
    }

    public bool DidCollisionHit(string collisionDescriptor)
    {
        CollisionCast2D collisionCast = GetCollisionResult(collisionDescriptor);
        return collisionCast.Hit;
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

        foreach (CollisionCast2D collision in collisions.Values)
        {
            collision.DrawGizmos();
        }
    }
}
