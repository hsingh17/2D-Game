using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private EntityScriptableObject scriptableObject;

    [SerializeReference, SubclassSelector]
    private List<CollisionCast2D> collisions;

    private StateManager<Enum> stateManager;
    private CollisionDetector2D collisionDetector2D;

    public abstract void CheckCollisions();
    public abstract void UpdateState();
    public abstract void DoAction();

    private void FixedUpdate()
    {
        CheckCollisions();
        UpdateState();
        DoAction();
    }
}
