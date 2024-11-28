using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 10, 0);
    public float smoothSpeed = 0.125f;
    public float panSpeed = 20f; // Speed of panning with the mouse
    public float zoomSpeed = 2f; // Speed of zooming
    public float minZoom = 0.5f; // Minimum zoom level
    public float maxZoom = 3f; // Maximum zoom level

    private float currentZoom = 10f;

    private void LateUpdate()
    {
        if (target != null)
        {
            // Follow the target
            Vector3 desiredPosition = target.position + offset * currentZoom;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }


        // Zoom the camera using the mouse scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentZoom = Mathf.Clamp(currentZoom - scroll * zoomSpeed, minZoom, maxZoom);
    }
}
