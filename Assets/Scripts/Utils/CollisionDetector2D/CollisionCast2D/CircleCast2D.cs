using System;
using UnityEngine;

[Serializable]
public class CircleCast2D : CollisionCast2D
{
    public float Radius;

    public CircleCast2D() { }

    public CircleCast2D(
        Collider2D collider,
        Vector2 direction,
        float distance,
        float radius,
        LayerMask mask,
        string descriptor
    )
    {
        Collider = collider;
        Direction = direction;
        Distance = distance;
        Radius = radius;
        Mask = mask;
        Descriptor = descriptor;
    }

    public override RaycastHit2D CheckCollision()
    {
        Collider2D collider = Collider;
        Bounds colliderBounds = collider.bounds;
        Vector3 colliderCenter = colliderBounds.center;
        return Physics2D.CircleCast(colliderCenter, Radius, Direction, Distance, Mask);
    }

    public override void DrawGizmos()
    {
        Vector3 center = Collider.bounds.center;
        center.z = 0;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(center, Radius);
        Gizmos.color = Hit ? Color.green : Color.red;
        center += new Vector3(Direction.x, Direction.y) * Distance;
        Gizmos.DrawWireSphere(center, Radius);

        base.DrawGizmos();
    }

    public override string ToString()
    {
        return $"{base.ToString()}\nRadius: {Radius}";
    }
}
