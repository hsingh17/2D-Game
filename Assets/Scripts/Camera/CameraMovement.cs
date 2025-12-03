using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float smoothTime;

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        Vector3 currentPos = transform.position;
        Vector3 targetPos = target.transform.position;

        currentPos.z = targetPos.z;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            smoothTime
        );
    }
}
