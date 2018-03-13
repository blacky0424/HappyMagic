using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScene : MonoBehaviour {

    [SerializeField]
    UnityEngine.UI.Text m_gameOverText;
    float m_color;

	void Start () {
        FadeManager.Instance.FadeIn();
        SoundManager.Instance.StopSE();
        SoundManager.Instance.StopBGM();

        m_color = 0;
        m_gameOverText.color = new Color(1.0f, 1.0f, 1.0f, m_color);
    }
	
	void Update () {

        if (m_color < 1.0f)
        {
            m_color += 0.01f;
        }
        else
        {
            m_color = 1.0f;

            if (Input.GetMouseButtonDown(0))
            {
                FadeManager.Instance.FadeOut();
                SceneManager.Instance.LoadScene(SceneManager.TitleScene);
            }
        }

        m_gameOverText.color = new Color(1.0f, 1.0f, 1.0f, m_color);
	}
}
