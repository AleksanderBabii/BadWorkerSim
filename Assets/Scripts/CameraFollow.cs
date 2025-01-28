using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("The player object the camera will follow.")]
    public Transform target;

    [Header("Camera Settings")]
    [Tooltip("Offset position relative to the target.")]
    public Vector3 offset = new Vector3(0f, 5f, -10f);

    [Tooltip("Smoothness of the camera movement.")]
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("CameraFollow script is missing a target to follow.");
            return;
        }

        // Calculate the desired position
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate between current position and desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update the camera position
        transform.position = smoothedPosition;

        // Optional: Rotate the camera to look at the target
        transform.LookAt(target);
    }
}
