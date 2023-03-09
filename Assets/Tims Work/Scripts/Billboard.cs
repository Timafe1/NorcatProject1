using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform _cameraTransform;

    void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        transform.LookAt(transform.position + _cameraTransform.rotation * Vector3.forward, _cameraTransform.rotation * Vector3.up);
    }
}
