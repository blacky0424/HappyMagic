using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinceEndCamera : MonoBehaviour {

    [SerializeField]
    ADVSystem adv;

    [SerializeField]
    int cameraRotateTime = 15;
    [SerializeField]
    int angle;

    float timer;
    bool isFinish;

    void Start ()
    {
        transform.Rotate(new Vector3(0,angle,0));
	}
	
	void Update ()
    {
        if (!adv.gameObject.activeSelf) { RotateCamera(cameraRotateTime); }
	}

    void RotateCamera(int timespan)
    {
        transform.Rotate(new Vector3(0, -(angle*2) / timespan, 0) * Time.deltaTime);
        timer += Time.deltaTime;
        if (!isFinish && timespan <= timer)
        {
            isFinish = true;
            SceneManager.Instance.LoadScene(SceneManager.TitleScene);
        }

    }
}
