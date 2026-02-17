using System;
using UnityEngine;

[Serializable]
public class RayCast2D : CollisionCast2D
{
    public RayCast2D() { }

    public RayCast2D(
        Collider2D collider,
        Vector2 direction,
        float distance,
        LayerMask mask,
        string descriptor
    )
    {
        Collider = collider;
        Direction = direction;
        Distance = distance;
        Mask = mask;
        Descriptor = descriptor;
    }

    public override RaycastHit2D CheckCollision()
    {
        Collider2D collider = Collider;
        Bounds colliderBounds = collider.bounds;
        Vector3 colliderCenter = colliderBounds.center;
        return Physics2D.Raycast(colliderCenter, Direction, Distance, Mask);
    }
}
