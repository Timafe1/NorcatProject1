using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform centerPoint; // the center point the camera revolves around
    public float distance = 10f; // the distance between the camera and center point
    public float speed = 5f; // the speed at which the camera revolves around the center point

    private Vector3 cameraPosition; // the current position of the camera

    void Start()
    {
        // set the initial camera position based on the distance and the center point
        cameraPosition = centerPoint.position - transform.forward * distance;
        transform.position = cameraPosition;
    }

    void Update()
    {
        // calculate the new camera position based on the center point and the speed
        cameraPosition = Quaternion.AngleAxis(Time.deltaTime * speed, Vector3.up) * (cameraPosition - centerPoint.position) + centerPoint.position;

        // set the camera's position to the new position
        transform.position = cameraPosition;

        // make the camera always look at the center point
        transform.LookAt(centerPoint.position);
    }
}
