using System;
using System.Collections.Generic;
using System.Data;
using NUnit.Framework.Interfaces;
using UnityEngine;

public class CollisionDetector2D : MonoBehaviour
{
    public bool DrawCollisions { get; set; }

    private readonly HashSet<CollisionCast2D> collisions;
    private readonly Dictionary<CollisionCast2D, CollisionDetect2D> collisionResults;

    public CollisionDetector2D(List<CollisionCast2D> collisions)
    {
        foreach (CollisionCast2D cast in collisions)
        {
            if (collisions.Contains(cast))
            {
                throw new Exception(
                    $"Duplicate collision with descriptor {cast.Descriptor} found!"
                );
            }
        }

        collisionResults = new();
    }

    public void CheckAllCollisions()
    {
        foreach (CollisionCast2D cast in collisions)
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

            collisionResults[cast] = collisionDetect2D;
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
