using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class CollisionDetector2D : MonoBehaviour
{
    public bool DrawCollisions { get; set; }

    private readonly List<CollisionCast2D> collisions;
    private readonly Dictionary<CollisionCast2D, CollisionDetect2D> collisionResults;

    public CollisionDetector2D(List<CollisionCast2D> collisions)
    {
        this.collisions = collisions;
    }

    public void CheckAllCollisions()
    {
        foreach (CollisionCast2D cast in collisions)
        {
            if (cast is BoxCast2D) { CheckBoxCollision(cast)}
            else if (cast is CircleCast2D) { CheckCircleCollision(cast)}
            else if (cast is RayCast2D) { CheckRayCastCollision(cast)}
            else
            {
                throw new NotImplementedException(
                    $"Unable to handle collision of class {cast.GetType().Name}"
                );
            }
        }
    }

    public CollisionDetect2D CheckBoxCollision(BoxCast2D boxCast2d)
    {
        return new CollisionDetect2D();
        Bounds colliderBounds = collider.bounds;
        Vector3 colliderCenter = colliderBounds.center;
        Vector3 colliderExtents = colliderBounds.extents;

        RaycastHit2D hit = Physics2D.BoxCast(
            colliderCenter,
            colliderExtents,
            angle,
            direction,
            distance,
            mask
        );

        float hitDistance = CalculateHitDistance(hit, collider, direction);

        return new CollisionDetect2D(hit, hitDistance);
    }

    public CollisionDetect2D CheckRayCastCollision(RayCast2D rayCast2D)
    {
        return new CollisionDetect2D();
        Bounds colliderBounds = collider.bounds;
        Vector3 colliderCenter = colliderBounds.center;

        RaycastHit2D hit = Physics2D.Raycast(colliderCenter, direction, distance, mask);

        float hitDistance = CalculateHitDistance(hit, collider, direction);

        return new CollisionDetect2D(hit, hitDistance);
    }

    public CollisionDetect2D CheckCircleCollision(CircleCast2D circleCast2D)
    {
        return new CollisionDetect2D();
        Bounds colliderBounds = collider.bounds;
        Vector3 colliderCenter = colliderBounds.center;
        Vector3 colliderExtents = colliderBounds.extents;

        RaycastHit2D hit = Physics2D.CircleCast(
            colliderCenter,
            colliderExtents.y,
            direction,
            radius,
            mask
        );

        float hitDistance = CalculateHitDistance(hit, collider, direction);
        return new CollisionDetect2D(hit, hitDistance);
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

    private void OnDrawGizmos() { }
}
