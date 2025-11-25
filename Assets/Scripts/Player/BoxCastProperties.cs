using System;
using UnityEngine;

[Serializable]
public struct BoxCastProperties
{
    public float OriginOffset;
    public float BoxCastDistance;
    public float BoxCastYSize;

    public BoxCastProperties(float originOffset, float boxCastDistance, float boxCastYSize)
    {
        OriginOffset = originOffset;
        BoxCastDistance = boxCastDistance;
        BoxCastYSize = boxCastYSize;
    }

    public Vector3 CalculateBoxCastOrigin(Vector3 colliderCenter, Vector3 colliderExtents)
    {
        return colliderCenter + (Vector3.down * (colliderExtents.y + OriginOffset));
    }

    public Vector3 CalculateBoxCastSize(Vector3 colliderSize)
    {
        return new(colliderSize.x, BoxCastYSize);
    }
}
