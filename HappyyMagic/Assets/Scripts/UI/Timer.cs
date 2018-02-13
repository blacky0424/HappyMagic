using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {


    //長針の指定
    public Transform Minute_hand;
    //短針の指定
    public Transform Hour_hand;
   
    float m_minute;
    float m_hour;

    
    
    // Use this for initialization
    void Start () {
        m_minute = 360 / GameScene.Instance.LimitTime / 60 ;
        m_hour = m_minute/12;
    }
	
	// Update is called once per frame
	void Update () {
        hourHand();
        minuteHund();
	}

    void hourHand() {

        Hour_hand.Rotate(new Vector3(0, 0, -m_hour));
    }

    void minuteHund() {
        Minute_hand.Rotate(new Vector3(0, 0, -m_minute ));
    }

}
