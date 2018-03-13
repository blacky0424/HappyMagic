using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScene : SingletonMonoBehaviour<TutorialScene>
{
    //チュートリアル画像
    [SerializeField]
	List<GameObject> m_tutorialList = new List<GameObject>();
    [SerializeField]
	GameObject m_nextButton;
    [SerializeField]
	GameObject m_backButton;
    [SerializeField]
	GameObject m_closeButton;
    //トリガーリスト
    [SerializeField]
    List<TutorialTrigger> m_triggerList = new List<TutorialTrigger>();
    //チュートリアルの状況
    public enum TutorialState
    {
        Tutorial1,
        Tutorial2,
        Tutorial3,
        Tutorial4,
        Tutorial5,
        Tutorial6,
        Tutorial7,
    }
    [HideInInspector]
    public TutorialState m_state;
    //リスタートの位置
    [SerializeField]
    List<Vector3> m_restartPos = new List<Vector3>();

	int m_num;

    void Start()
    {
        //初期化
        for(int i = 0; i < m_tutorialList.Count; i++)
        {
            m_tutorialList[i].SetActive(false);
        }
        m_nextButton.SetActive(false);
        m_backButton.SetActive(false);
        m_closeButton.SetActive(false);

        Invoke(((System.Action)Tutorial1).Method.Name, 1.0f);
    }

    /// <summary>
    /// チュートリアル1
    /// </summary>
    public void Tutorial1()
    {
        m_state = TutorialState.Tutorial1;
        m_num = 0;
        m_nextButton.SetActive(true);
        for (int i = 0; i < 2; i++)
        {
            m_tutorialList[i].SetActive(true);
        }
        Time.timeScale = 0;
    }

    /// <summary>
    /// チュートリアル2
    /// </summary>
    public void Tutorial2(){
        m_state = TutorialState.Tutorial2;
        m_num = 2;
        m_tutorialList[m_num].SetActive(true);
        m_closeButton.GetComponent<RectTransform>().localPosition = new Vector3(0, -426.0f, 0);
        m_closeButton.SetActive(true);
        Time.timeScale = 0;
    }

	/// <summary>
	/// チュートリアル3
	/// </summary>
	public void Tutorial3(){
        m_state = TutorialState.Tutorial3;
        m_num = 3;
		m_tutorialList[m_num].SetActive(true);
		m_closeButton.SetActive (true);
		Time.timeScale = 0;
	}

	/// <summary>
	/// チュートリアル4
	/// </summary>
	public void Tutorial4(){
        m_state = TutorialState.Tutorial4;
        m_num = 4;
		m_tutorialList[m_num].SetActive(true);
		m_closeButton.SetActive (true);
		Time.timeScale = 0;
	}

	/// <summary>
	/// チュートリアル5
	/// </summary>
	public void Tutorial5(){
        m_state = TutorialState.Tutorial5;
        m_num = 5;
		m_tutorialList[m_num].SetActive(true);
		m_closeButton.SetActive (true);
		Time.timeScale = 0;
	}

	/// <summary>
	/// チュートリアル6
	/// </summary>
	public void Tutorial6(){
        m_state = TutorialState.Tutorial6;
        m_num = 6;
		m_tutorialList[m_num].SetActive(true);
		m_closeButton.SetActive (true);
		Time.timeScale = 0;
	}

	/// <summary>
	/// チュートリアル7
	/// </summary>
	public void Tutorial7(){
        m_state = TutorialState.Tutorial7;
        m_num = 7;
		m_tutorialList[m_num].SetActive(true);
		m_closeButton.SetActive (true);
		Time.timeScale = 0;
	}

	/// <summary>
	/// ネクストボタン
	/// </summary>
	public void Next(){
		m_tutorialList[m_num].SetActive(false);
		switch (m_num) {
		case 0:
			m_backButton.SetActive (true);
			m_nextButton.SetActive (false);
			m_closeButton.SetActive (true);
			break;
		case 1:

			break;
		}
		m_num++;
	}

	/// <summary>
	/// バックボタン
	/// </summary>
	public void Back(){
		m_num--;
		m_tutorialList[m_num].SetActive(true);
		switch (m_num) {
		case 0:
            m_nextButton.SetActive(true);
            m_backButton.SetActive (false);
            m_closeButton.SetActive(false);
            break;
		case 1:

			break;
		}
	}

	/// <summary>
	/// クローズボタン
	/// </summary>
	public void Close(){
		for(int i = 0; i < m_tutorialList.Count; i++)
		{
			m_tutorialList[i].SetActive(false);
		}
		m_nextButton.SetActive(false);
		m_backButton.SetActive(false);
		m_closeButton.SetActive(false);

		Time.timeScale = 1.0f;
	}

    /// <summary>
    /// リスタートの処理
    /// </summary>
    public void Restart()
    {
        GameScene.Instance.Cinderella.transform.position = m_restartPos[(int)m_state];
        m_triggerList[(int)(m_state - 1)].Restart();
    }
}
