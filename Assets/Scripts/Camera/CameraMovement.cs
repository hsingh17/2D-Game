using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private float smoothTime;

    [SerializeField]
    private BoxCollider2D worldBounds;

    private Vector3 velocity = Vector3.zero;
    private Camera cam;
    private Vector2 camExtents;

    private void Awake()
    {
        cam = Camera.main;
        camExtents = new(cam.aspect * cam.orthographicSize, cam.orthographicSize);
    }

    private void LateUpdate()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        Vector3 targetPos = RestrictCameraToBounds(target.transform.position + offset);
        Vector3 currentPos = transform.position;
        currentPos.z = targetPos.z;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            smoothTime
        );
    }

    private Vector3 RestrictCameraToBounds(Vector3 targetPos)
    {
        targetPos.y += CalculateBoundsDelta(targetPos + Vector3.up * camExtents.y).y; // Top
        targetPos.y += CalculateBoundsDelta(targetPos + Vector3.down * camExtents.y).y; // Bottom
        targetPos.x += CalculateBoundsDelta(targetPos + camExtents.x * Vector3.left).x; // Left
        targetPos.x += CalculateBoundsDelta(targetPos + camExtents.x * Vector3.right).x; // Right
        return targetPos;
    }

    private Vector3 CalculateBoundsDelta(Vector3 point)
    {
        Bounds bounds = worldBounds.bounds;
        return bounds.ClosestPoint(point) - point;
    }
}
