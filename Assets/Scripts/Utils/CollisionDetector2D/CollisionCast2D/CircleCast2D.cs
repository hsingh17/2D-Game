using UnityEngine;

public class CircleCast2D : ICollisionCast2D
{
    public Collider2D Collider { get; set; }
    public Vector2 Direction { get; set; }
    public float Radius { get; set; }
    public LayerMask Mask { get; set; }

    public CircleCast2D(Collider2D collider, Vector2 direction, float radius, LayerMask mask)
    {
        Collider = collider;
        Direction = direction;
        Radius = radius;
        Mask = mask;
    }
}
