using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
    }

    void LateUpdate()
    {
        transform.LookAt(playerCamera.transform);
        transform.rotation = Quaternion.LookRotation(playerCamera.transform.forward);
    }
}
