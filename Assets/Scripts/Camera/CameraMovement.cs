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
    private Tilemap backgroundTilemap;

    private Vector3 velocity = Vector3.zero;
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        // Don't move camera if not inside the bounds of the background
        if (!TileMapContainsCamera())
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

    private bool TileMapContainsCamera()
    {
        Bounds tilemapBounds = backgroundTilemap.localBounds;
        Vector3 currentPos = transform.position;
        currentPos.z = tilemapBounds.center.z;

        Vector3 top = currentPos + Vector3.up * cam.orthographicSize;
        Vector3 bottom = currentPos + Vector3.down * cam.orthographicSize;
        Vector3 left = currentPos + cam.aspect * cam.orthographicSize * Vector3.left;
        Vector3 right = currentPos + cam.aspect * cam.orthographicSize * Vector3.right;

        return tilemapBounds.Contains(top)
            && tilemapBounds.Contains(bottom)
            && tilemapBounds.Contains(left)
            && tilemapBounds.Contains(right);
    }
}
