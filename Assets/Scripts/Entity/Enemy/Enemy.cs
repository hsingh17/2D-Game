using System;
using UnityEngine;

public abstract class Enemy<T> : Entity<T>
    where T : Enum
{
    [SerializeField]
    protected CircleCollider2D[] patrolPoints;

    [SerializeField]
    protected float restTime;

    protected int currentPatrolPointIdx = 0;
}
