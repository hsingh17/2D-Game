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
        // Don't move camera if not inside the bounds of the background
        if (!ClampCameraToBounds())
        {
            return;
        }

        // TODO: Camera still moves ever so slightly out of bounds. Need to do clamping instead I think

        Vector3 targetPos = target.transform.position + offset;
        Vector3 currentPos = transform.position;
        currentPos.z = targetPos.z;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            smoothTime
        );
    }

    private bool ClampCameraToBounds()
    {
        Bounds bounds = worldBounds.bounds;
        Vector3 currentPos = transform.position;
        currentPos.z = bounds.center.z;

        Vector3 top = currentPos + Vector3.up * camExtents.y;
        Vector3 bottom = currentPos + Vector3.down * camExtents.y;
        Vector3 left = currentPos + camExtents.x * Vector3.left;
        Vector3 right = currentPos + camExtents.x * Vector3.right;

        return bounds.Contains(top)
            && bounds.Contains(bottom)
            && bounds.Contains(left)
            && bounds.Contains(right);
    }
}
