using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCameraController : MonoBehaviour
{
    Camera arCamera;

    // Start is called before the first frame update
    void Start()
    {
        arCamera = Camera.main;
    }

    void LateUpdate()
    {
        transform.LookAt(arCamera.transform);
        transform.rotation = Quaternion.LookRotation(arCamera.transform.forward);
    }
}
