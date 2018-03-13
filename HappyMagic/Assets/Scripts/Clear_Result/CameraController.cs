using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 rotationOffset;
    bool hasInitialized;

    void Start()
    {
        transform.localRotation = GyroManager.Instance.GyroAttitude;
        rotationOffset = new Vector3(0.0f, transform.localEulerAngles.y, 0.0f);
        transform.localEulerAngles -= rotationOffset;
    }

    void Update()
    {
        Quaternion gyro = Input.gyro.attitude;
        gyro = Quaternion.Euler(90.0f, 0.0f, 0.0f) * (new Quaternion(-gyro.x, -gyro.y, gyro.z, gyro.w));
        transform.localRotation = gyro;
        transform.localEulerAngles -= rotationOffset;
    }
}