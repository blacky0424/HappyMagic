using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroManager : SingletonMonoBehaviour<GyroManager>
{
    public Quaternion GyroAttitude { get; private set; }

    private void Start()
    {
        DontDestroyOnLoad(this);
        Input.gyro.enabled = true;
    }

    private void Update()
    {
        Quaternion gyro = Input.gyro.attitude;
        GyroAttitude = Quaternion.Euler(90, 0, 0) * (new Quaternion(-gyro.x, -gyro.y, gyro.z, gyro.w));
    }
}
