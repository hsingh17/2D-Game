using System;
using UnityEngine;

[Serializable]
public abstract class CollisionCast2D
{
    public string Descriptor;
    public Collider2D Collider;

    public Vector2 Direction;

    public LayerMask Mask;

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
}
