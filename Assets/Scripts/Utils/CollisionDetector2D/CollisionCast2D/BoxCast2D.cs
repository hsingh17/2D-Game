using UnityEngine;

public struct BoxCast2D : ICollisionCast2D
{
    public Collider2D Collider { get; set; }
    public float Angle { get; set; }
    public Vector2 Direction { get; set; }
    public float Distance { get; set; }
    public LayerMask Mask { get; set; }

    public BoxCast2D(
        Collider2D collider,
        float angle,
        Vector2 direction,
        float distance,
        LayerMask mask
    )
    {
        Collider = collider;
        Angle = angle;
        Direction = direction;
        Distance = distance;
        Mask = mask;
    }
}
