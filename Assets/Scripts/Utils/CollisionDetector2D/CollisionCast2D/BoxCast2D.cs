using System;
using UnityEngine;

[Serializable]
public class BoxCast2D : CollisionCast2D
{
    public float Angle;

    public BoxCast2D() { }

    public BoxCast2D(
        Collider2D collider,
        float angle,
        Vector2 direction,
        float distance,
        LayerMask mask,
        string descriptor
    )
    {
        Collider = collider;
        Angle = angle;
        Direction = direction;
        Distance = distance;
        Mask = mask;
        Descriptor = descriptor;
    }

    public override RaycastHit2D CheckCollision()
    {
        Collider2D collider = Collider;
        Bounds colliderBounds = collider.bounds;
        return Physics2D.BoxCast(
            colliderBounds.center,
            colliderBounds.extents,
            Angle,
            Direction,
            Distance,
            Mask
        );
    }

    public override void DrawGizmos()
    {
        Vector3 center = Collider.bounds.center;
        Vector3 extents = Collider.bounds.extents;
        Vector3 size = new(extents.x, extents.y, 0.01f);
        center.z = 0;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(center, size);
        Gizmos.color = Hit ? Color.green : Color.red;
        center += new Vector3(Direction.x, Direction.y) * Distance;
        Gizmos.DrawWireCube(center, size);

        base.DrawGizmos();
    }

    public override string ToString()
    {
        return $"{base.ToString()}\nAngle: {Angle}";
    }
}
