using System;
using UnityEngine;

[Serializable]
public abstract class CollisionCast2D
{
    public string Descriptor;
    public Collider2D Collider;
    public Vector2 Direction;
    public LayerMask Mask;
    public float Distance;
    public bool Hit;
    public float HitDistance;

    public abstract RaycastHit2D CheckCollision();

    public virtual void DrawGizmos()
    {
        // If collision detected, draw the point where it was hit
        if (Hit)
        {
            Gizmos.color = Color.green;
            Vector3 center = Collider.bounds.center;
            center += new Vector3(Direction.x, Direction.y) * HitDistance;
            Gizmos.DrawSphere(center, 0.2f);
        }
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        CollisionCast2D other = obj as CollisionCast2D;
        return Descriptor.Equals(other.Descriptor);
    }

    public override int GetHashCode()
    {
        return Descriptor.GetHashCode();
    }

    public override string ToString()
    {
        return $"Descriptor: {Descriptor}\nCollider: {Collider.name}\n"
            + $"Direction: {Direction}\nDistance: {Distance}\nMask: {LayerMask.LayerToName(Mask.value)}\n";
    }
}
