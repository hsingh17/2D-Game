using System;
using UnityEngine;

[Serializable]
public class CircleCast2D : CollisionCast2D
{
    public Collider2D Collider;
    public Vector2 Direction;
    public float Radius;
    public LayerMask Mask;

    public CircleCast2D() { }

    public CircleCast2D(
        Collider2D collider,
        Vector2 direction,
        float radius,
        LayerMask mask,
        string descriptor
    )
    {
        Collider = collider;
        Direction = direction;
        Radius = radius;
        Mask = mask;
        Descriptor = descriptor;
    }
}
