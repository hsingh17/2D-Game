using System;
using UnityEngine;

public abstract class Enemy<T> : Entity<T>
    where T : Enum
{
    [SerializeField]
    protected GameObject patrolPointA;

    [SerializeField]
    protected GameObject patrolPointB;

    [SerializeField]
    protected float restTime;
}
