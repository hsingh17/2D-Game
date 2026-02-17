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

    public abstract RaycastHit2D CheckCollision();
    public abstract void DrawGizmos();

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
