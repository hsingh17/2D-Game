using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity<T> : MonoBehaviour
    where T : Enum
{
    [SerializeField]
    protected Rigidbody2D rb;

    [SerializeField]
    protected EntityScriptableObject scriptableObject;

    [SerializeField]
    protected bool drawCollisions;

    [SerializeReference, SubclassSelector]
    protected List<CollisionCast2D> collisions;

    protected CollisionDetector2D collisionDetector2D;

    protected abstract StateManager<T> StateManager { get; }
    protected abstract void UpdateState();
    protected abstract void DoAction();

    protected virtual void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        collisionDetector2D = gameObject.AddComponent<CollisionDetector2D>();
        collisionDetector2D.DrawCollisions = drawCollisions;
        collisionDetector2D.AddCollisions(collisions);
    }

    protected virtual void CheckCollisions()
    {
        collisionDetector2D.CheckAllCollisions();
    }

    private void FixedUpdate()
    {
        CheckCollisions();
        UpdateState();
        DoAction();
    }
}
