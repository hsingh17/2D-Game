using System;
using UnityEngine;

public abstract class Enemy<T> : Entity<T>
    where T : Enum
{
    [SerializeField]
    protected CircleCollider2D leftPatrolPoint;

    [SerializeField]
    protected CircleCollider2D rightPatrolPoint;

    [SerializeField]
    protected float restTime;
}
