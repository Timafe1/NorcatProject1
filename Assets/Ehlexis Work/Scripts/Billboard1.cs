using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard1 : MonoBehaviour
{
    private Transform _cameraTransform;

    void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        Vector3 cameraForward = _cameraTransform.forward;
        cameraForward.x = 0;
        cameraForward.y = 0;// Set X to 0 to only use the YZ-plane rotation
        transform.LookAt(transform.position + _cameraTransform.rotation * cameraForward, _cameraTransform.rotation * Vector3.up);
    }
}