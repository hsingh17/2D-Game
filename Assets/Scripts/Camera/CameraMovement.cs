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
    private Camera camera;

    private void Start()
    {
        camera = gameObject.GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        Debug.Log($"{camera.orthographicSize}");
        // Don't move camera if not inside the bounds of the background
        if (!TileMapContainsCamera())
        {
            return;
        }

        Vector3 currentPos = transform.position;
        Vector3 targetPos = target.transform.position + offset;

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
        return true;
        // Bounds tilemapBounds = backgroundTilemap.localBounds;
        // Vector3 currentPos = transform.position;
        // Vector3 top = transform.position;
        // Vector3 bottom;
        // Vector3 left;
        // Vector3 right;
        // return tilemapBounds.Contains()
        //     && tilemapBounds.Contains()
        //     && tilemapBounds.Contains()
        //     && tilemapBounds.Contains();
    }
}
