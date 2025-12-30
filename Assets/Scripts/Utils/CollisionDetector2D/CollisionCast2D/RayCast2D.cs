using UnityEngine;

public class RayCast2D : ICollisionCast2D
{
    public Collider2D Collider { get; set; }
    public Vector2 Direction { get; set; }
    public float Distance { get; set; }
    public LayerMask Mask { get; set; }

    public RayCast2D(Collider2D collider, Vector2 direction, float distance, LayerMask mask)
    {
        Collider = collider;
        Direction = direction;
        Distance = distance;
        Mask = mask;
    }
}
