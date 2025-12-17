using UnityEngine;

public class CollisionDetector2D
{
    public struct CollisionDetect2D
    {
        public bool Hit { get; }
        public float HitDistance { get; }

        public CollisionDetect2D(bool hit, float hitDistance)
        {
            Hit = hit;
            HitDistance = hitDistance;
        }
    };

    public static CollisionDetect2D CheckCircleCollision2D(
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

    private static float CalculateHitDistance(
        RaycastHit2D hit,
        Collider2D collider,
        Vector2 direction
    )
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
}
