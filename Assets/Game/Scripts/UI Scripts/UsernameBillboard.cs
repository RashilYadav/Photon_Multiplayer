using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsernameBillboard : MonoBehaviour
{
    Camera mainCam;

    private void Update()
    {
        if(mainCam == null)
        {
            mainCam = FindObjectOfType<Camera>();
        }

        if(mainCam == null)
        {
            return;
        }

        // making the "Username & Team" board look at the camera
        transform.LookAt(mainCam.transform);
        transform.Rotate(Vector3.up * 180);
    }
}
