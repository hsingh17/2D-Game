using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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
        Vector2 curPosition = transform.position;
        rb.MovePosition(
            curPosition + movement * entityScriptableObject.speed * Time.fixedDeltaTime
        );
    }

    private void Jump() { }
}
