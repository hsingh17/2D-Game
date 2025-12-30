using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector2D : MonoBehaviour
{
    public bool DrawCollisions { get; set; }

    public CollisionDetect2D CheckBoxCollision(
        Collider2D collider,
        float angle,
        Vector2 direction,
        float distance,
        LayerMask mask
    )
    {
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

    public CollisionDetect2D CheckRayCastCollision(
        Collider2D collider,
        Vector2 direction,
        float distance,
        LayerMask mask
    )
    {
        Bounds colliderBounds = collider.bounds;
        Vector3 colliderCenter = colliderBounds.center;

        RaycastHit2D hit = Physics2D.Raycast(colliderCenter, direction, distance, mask);

        float hitDistance = CalculateHitDistance(hit, collider, direction);

        return new CollisionDetect2D(hit, hitDistance);
    }

    public CollisionDetect2D CheckCircleCollision(
        Collider2D collider,
        Vector2 direction,
        float radius,
        LayerMask mask
    )
    {
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
