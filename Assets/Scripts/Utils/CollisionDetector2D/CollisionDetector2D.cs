using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
            CollisionDetect2D collisionDetect2D;

            if (cast is BoxCast2D boxCast2D)
            {
                collisionDetect2D = CheckBoxCollision(boxCast2D);
            }
            else if (cast is CircleCast2D circleCast2D)
            {
                collisionDetect2D = CheckCircleCollision(circleCast2D);
            }
            else if (cast is RayCast2D rayCast2D)
            {
                collisionDetect2D = CheckRayCastCollision(rayCast2D);
            }
            else
            {
                throw new NotImplementedException(
                    $"Unable to handle collision of class {cast.GetType().Name}"
                );
            }

            collisionResults[cast.Descriptor] = collisionDetect2D;
        }
    }

    public CollisionDetect2D CheckBoxCollision(BoxCast2D boxCast2D)
    {
        Collider2D collider = boxCast2D.Collider;
        Bounds colliderBounds = collider.bounds;
        Vector3 colliderCenter = colliderBounds.center;
        Vector3 colliderExtents = colliderBounds.extents;

        RaycastHit2D hit = Physics2D.BoxCast(
            colliderCenter,
            colliderExtents,
            boxCast2D.Angle,
            boxCast2D.Direction,
            boxCast2D.Distance,
            boxCast2D.Mask
        );

        float hitDistance = CalculateHitDistance(hit, collider, boxCast2D.Direction);

        return new CollisionDetect2D(hit, hitDistance);
    }

    public CollisionDetect2D CheckRayCastCollision(RayCast2D rayCast2D)
    {
        Collider2D collider = rayCast2D.Collider;
        Bounds colliderBounds = collider.bounds;
        Vector3 colliderCenter = colliderBounds.center;

        RaycastHit2D hit = Physics2D.Raycast(
            colliderCenter,
            rayCast2D.Direction,
            rayCast2D.Distance,
            rayCast2D.Mask
        );

        float hitDistance = CalculateHitDistance(hit, collider, rayCast2D.Direction);

        return new CollisionDetect2D(hit, hitDistance);
    }

    public CollisionDetect2D CheckCircleCollision(CircleCast2D circleCast2D)
    {
        Collider2D collider = circleCast2D.Collider;
        Bounds colliderBounds = collider.bounds;
        Vector3 colliderCenter = colliderBounds.center;
        Vector3 colliderExtents = colliderBounds.extents;

        RaycastHit2D hit = Physics2D.CircleCast(
            colliderCenter,
            colliderExtents.y,
            circleCast2D.Direction,
            circleCast2D.Radius,
            circleCast2D.Mask
        );

        float hitDistance = CalculateHitDistance(hit, collider, circleCast2D.Direction);
        return new CollisionDetect2D(hit, hitDistance);
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
        else
        {
            return null;
        }
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

            DrawCast(collision);
        }
    }

    private void DrawCast(CollisionCast2D collision)
    {
        Vector3 center = collision.Collider.bounds.center;
        Vector3 extents = collision.Collider.bounds.extents;
        Vector3 size = new(extents.x, extents.y, 0.01f);
        if (collision is BoxCast2D)
        {
            Gizmos.DrawCube(center, size);
        }
        else if (collision is CircleCast2D circleCast)
        {
            Gizmos.DrawSphere(center, circleCast.Radius);
        }
        else if (collision is RayCast2D rayCast)
        {
            Vector3 toVector =
                center
                + (
                    new Vector3(collision.Direction.x, collision.Direction.y, 0.01f)
                    * rayCast.Distance
                );
            Gizmos.DrawRay(center, toVector);
        }
        else
        {
            Debug.LogWarning($"Unable to draw collision of class {collision.GetType().Name}");
        }
    }
}
