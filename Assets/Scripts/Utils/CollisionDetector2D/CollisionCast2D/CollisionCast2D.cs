using System;

[Serializable]
public abstract class CollisionCast2D
{
    public string Descriptor;

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
