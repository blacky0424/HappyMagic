using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMassage : MonoBehaviour {

    [SerializeField]
    UnityEngine.UI.Text m_startMessageText;
    [SerializeField]
    UnityEngine.UI.Image m_startImage;
    [SerializeField]
    Vector3 m_clearConditionPos;
    float m_alpha;
    bool m_Isfadeout;
    Transform messageRect;

	void Start () {
        StartCoroutine(FadeOutTime());
        m_alpha = 1.0f;
        m_Isfadeout = false;
        m_startMessageText.color = new Color(1.0f, 1.0f, 1.0f, m_alpha);
        m_startImage.color = new Color(1.0f, 1.0f, 1.0f, m_alpha);
        messageRect = GetComponent<RectTransform>();
    }

    IEnumerator FadeOutTime()
    {
        yield return new WaitForSeconds(3.0f);
        m_Isfadeout = true;
    }

	void Update () {
        if(m_alpha < 0f)
        {                     
            m_Isfadeout = false;
            MoveMessage();
        }
        if (m_Isfadeout)
        {
            m_alpha -= 0.005f;
            m_startMessageText.color = new Color(1.0f, 1.0f, 1.0f, m_alpha);
            m_startImage.color = new Color(1.0f, 1.0f, 1.0f, m_alpha);
        }
    }

    void MoveMessage()
    {
        m_alpha = 1.0f;
        m_startMessageText.color = new Color(1.0f, 1.0f, 1.0f, m_alpha);
        m_startImage.color = new Color(1.0f, 1.0f, 1.0f, m_alpha);
        messageRect.localPosition = m_clearConditionPos;
        messageRect.localScale = new Vector3(0.6f, 0.6f, 1.0f);
    }
}
