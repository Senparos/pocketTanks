using UnityEngine;

public class BoundsCheck : MonoBehaviour
{
    [Header("Map Boundaries")]
    public float xMin = -25f; // Left boundary
    public float xMax = 25f;  // Right boundary
    public float zMin = -25f; // Bottom boundary
    public float zMax = 25f;  // Top boundary

    private Vector3 clampedPosition;

    void Update()
    {
        CheckBounds();
    }

    void CheckBounds()
    {
        // Get the current position of the object
        Vector3 position = transform.position;

        // Clamp the position to stay within the defined boundaries
        clampedPosition = new Vector3(
            Mathf.Clamp(position.x, xMin, xMax),
            position.y, // Preserve Y-axis (height)
            Mathf.Clamp(position.z, zMin, zMax)
        );

        // Apply the clamped position back to the object
        transform.position = clampedPosition;
    }
}
