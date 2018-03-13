using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>{


    int m_Score = 0;
    float m_Time = 0;
    int m_FootPrint = 0;
    float m_GrassBreak = 0;
    int m_Discovery = 0;


    public int Score {
        get { return m_Score; }
    }

    public int FootPrint {
        get { return m_FootPrint; }
        set { m_FootPrint = value; }
    }

    public float time {
        get { return m_Time; }
        set { m_Time = value; }
    }

    public float GrassBreak
    {
        get { return m_GrassBreak; }
        set { m_GrassBreak = value; }
    }

    public int discovery {
        get { return m_Discovery; }
        set { m_Discovery = value; }
    }

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetTimeScore() {
        float p = Mathf.Round((GameScene.Instance.LimitTime - GameScene.Instance.elapsedtime) * 10);
        //小数点第2位を四捨五入した値
        m_Time = p / 10;        
    }

    public void SetDiscoveryScore() {
        m_Discovery = 3 - GameScene.Instance.lifecount;       
    }

    public void ScoreCalculation() {
        m_Score = (int)(m_Time * 10) + (m_FootPrint * 300) + (int)(m_GrassBreak * 500) - (m_Discovery * 1000);
    }

    public void ResetPoint() {
        m_Score = 0;
        m_Discovery = 0;
        m_FootPrint = 0;
        m_Time = 0;
        m_GrassBreak = 0;
    }
}
