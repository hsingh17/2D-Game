public struct CollisionDetect2D
{
    public bool Hit { get; }
    public float HitDistance { get; set; }

    public CollisionDetect2D(bool hit, float hitDistance)
    {
        Hit = hit;
        HitDistance = hitDistance;
    }
};
