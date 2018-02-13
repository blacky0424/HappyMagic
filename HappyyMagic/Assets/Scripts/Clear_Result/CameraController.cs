using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    Transform cameratransform;
    Quaternion gyro;

    // 調整値
    readonly Quaternion _BASE_ROTATION = Quaternion.Euler(90, 0, 0);

    void Start()
    {
        cameratransform = transform;
    }

    void Update()
    {
        Input.gyro.enabled = true;
        if (Input.gyro.enabled)
        {
            gyro = Input.gyro.attitude;

            //カメラの回転をジャイロを元に調整して設定
            cameratransform.localRotation = _BASE_ROTATION * (new Quaternion(-gyro.x, -gyro.y, gyro.z, gyro.w));
        }
    }
}