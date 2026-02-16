using System;
using UnityEngine;

[Serializable]
public class RayCast2D : CollisionCast2D
{
    public float Distance;

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

    public override string ToString()
    {
        return $"{base.ToString()}\nDistance: {Distance}";
    }
}
