using System;
using UnityEngine;

[Serializable]
public class BoxCast2D : CollisionCast2D
{
    public float Angle;
    public float Distance;

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

    public override string ToString()
    {
        return $"{base.ToString()}\nAngle: {Angle}\nDistance: {Distance}";
    }
}
