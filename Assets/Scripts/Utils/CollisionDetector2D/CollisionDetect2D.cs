public struct CollisionDetect2D
{
    public bool Hit { get; }
    public float HitDistance { get; set; }

    public CollisionDetect2D(bool hit, float hitDistance)
    {
        Hit = hit;
        HitDistance = hitDistance;
    }

    public override readonly string ToString()
    {
        if (Hit)
        {
            return $"Collision detected at {HitDistance} units away";
        }
        else
        {
            return "No collision detected";
        }
    }
};
