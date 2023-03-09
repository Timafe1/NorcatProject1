using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;                    // The transform of the object that the camera will follow
    public float distance = 10f;                // The distance between the camera and the target object
    public float height = 5f;                   // The height of the camera above the target object
    public float smoothSpeed = 0.125f;          // The smoothing speed for camera movement
    public float rotationSpeed = 2f;            // The rotation speed for the camera

    private Vector3 offset;                     // The offset between the camera and the target object
    private float mouseX, mouseY;               // The current mouse X and Y positions
    private float startDistance, startHeight;   // The starting distance and height values

    void Start()
    {
        // Set the initial camera position based on the distance and height values
        transform.position = target.position - Vector3.forward * distance + Vector3.up * height;

        // Calculate the offset between the camera and the target object
        offset = transform.position - target.position;

        // Store the starting distance and height values
        startDistance = distance;
        startHeight = height;
    }

    void LateUpdate()
    {
        // Calculate the current mouse X and Y positions for camera rotation
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -30f, 60f);

        // Rotate the camera around the target object based on the mouse X and Y positions
        Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0f);
        transform.position = target.position + rotation * offset;
        transform.LookAt(target);

        // Smoothly move the camera position towards the desired position
        Vector3 desiredPosition = target.position - rotation * Vector3.forward * distance + Vector3.up * height;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }

    // Function to set the camera distance and height values
    public void SetDistanceAndHeight(float newDistance, float newHeight)
    {
        distance = newDistance;
        height = newHeight;

        // Update the camera position based on the new distance and height values
        transform.position = target.position - Vector3.forward * distance + Vector3.up * height;

        // Calculate the new offset between the camera and the target object
        offset = transform.position - target.position;
    }

    // Function to reset the camera distance and height values to their starting values
    public void ResetDistanceAndHeight()
    {
        distance = startDistance;
        height = startHeight;

        // Update the camera position based on the starting distance and height values
        transform.position = target.position - Vector3.forward * distance + Vector3.up * height;

        // Calculate the new offset between the camera and the target object
        offset = transform.position - target.position;
    }
}
