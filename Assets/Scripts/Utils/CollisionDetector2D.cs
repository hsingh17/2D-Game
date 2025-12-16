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
        // if (hit)
        // {
        //     isGrounded = true;
        //     displacementToGround = hit.point.y - (colliderCenter.y - colliderExtents.y);
        // }
        // else
        // {
        //     isGrounded = false;
        // }
        return 0;
    }
}
