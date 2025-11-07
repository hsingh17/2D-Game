using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public delegate void OnPlayerMove(PlayerState state);
    public static event OnPlayerMove onPlayerMove;

    [SerializeField]
    private EntityScriptableObject entityScriptableObject;

    [SerializeField]
    private InputActionReference moveAction;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private bool isGrounded;

    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private CapsuleCollider2D groundCheck;

    private Vector2 movement;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        movement = moveAction.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        CheckGrounded();
        Move();
        PublishOnPlayerMoveMessage();
    }

    private void CheckGrounded()
    {
        Vector2 center = groundCheck.bounds.center;
        Collider2D[] colliders = Physics2D.OverlapCapsuleAll(
            center,
            groundCheck.size,
            CapsuleDirection2D.Horizontal,
            0,
            groundMask
        );
        isGrounded = colliders.Length > 0;
    }

    private void Move()
    {
        Vector2 curPosition = rb.position;
        Vector2 change = new(entityScriptableObject.speed * Time.fixedDeltaTime, GetYMovement());
        if (!isGrounded)
        {
            movement.y = 1;
        }

        rb.MovePosition(curPosition + (change * movement));
    }

    private void PublishOnPlayerMoveMessage()
    {
        if (movement == Vector2.zero) // Idle
        {
            onPlayerMove.Invoke(PlayerState.Idle);
        }
        else if (movement.y == 0 && movement.x != 0) // Left or Right movement
        {
            onPlayerMove.Invoke(movement.x > 0 ? PlayerState.MoveRight : PlayerState.MoveLeft);
        }
        //  Falling and Jumping not both working only one or the other
        else if (!isGrounded) // Fall
        {
            onPlayerMove.Invoke(PlayerState.Fall);
        }
        else if (movement.y > 0) // Jump
        {
            onPlayerMove.Invoke(PlayerState.Jump);
        }
    }

    private float GetYMovement()
    {
        if (isGrounded && movement.y != 0)
        {
            return SolveKinematicsEquation(entityScriptableObject.jumpSpeed, 0);
        }
        else if (!isGrounded)
        {
            return GetGravityMovement();
        }
        else
        {
            return 0;
        }
    }

    private float GetGravityMovement()
    {
        if (isGrounded)
        {
            return 0;
        }

        return SolveKinematicsEquation(
            rb.linearVelocityY,
            entityScriptableObject.gravityForce * entityScriptableObject.gravityScale
        );
    }

    private float SolveKinematicsEquation(float velocity, float acceleration)
    {
        return (velocity * Time.fixedDeltaTime)
            + (0.5f * acceleration * Time.fixedDeltaTime * Time.fixedDeltaTime);
    }
}
