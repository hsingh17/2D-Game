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
            CollisionDetect2D collisionDetect2D = new(hit, hitDistance);
            collisionResults[cast.Descriptor] = collisionDetect2D;
            Logger.Log($"{cast}\n{collisionDetect2D}");
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
            // In blue, draw the start of the cast
            Gizmos.color = Color.blue;
            DrawCast(collision);

            // Now draw the end of the cast
            CollisionDetect2D? detect = GetCollisionResult(collision.Descriptor);

            // Green for hit
            if (detect.HasValue && detect.Value.Hit)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }

            DrawCast(collision, false);
        }
    }

    private void DrawCast(CollisionCast2D collision, bool isStart = true)
    {
        Vector3 center = collision.Collider.bounds.center;
        Vector3 extents = collision.Collider.bounds.extents;
        Vector3 size = new(extents.x, extents.y, 0.01f);

        // We're drawing the end of the cast, so we need to add distance travelled
        if (!isStart)
        {
            center += (
                new Vector3(collision.Direction.x, collision.Direction.y) * collision.Distance
            );
        }

        center.z = 0;
        if (collision is BoxCast2D)
        {
            Gizmos.DrawWireCube(center, size);
        }
        else if (collision is CircleCast2D circleCast)
        {
            Gizmos.DrawWireSphere(center, circleCast.Radius);
        }
        else if (collision is RayCast2D)
        {
            Gizmos.DrawRay(center, collision.Direction);
        }
        else
        {
            Logger.LogWarning($"Unable to draw collision of class {collision.GetType().Name}");
        }
    }
}
