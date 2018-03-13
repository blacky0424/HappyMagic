using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinderellaIconManager : MonoBehaviour {

    List<IconController> m_iconBox = new List<IconController>();

    void Start()
    {
        //子オブジェクトの取得(上から順番)
        IconController[] children = GetComponentsInChildren<IconController>();
        for (int i = 0; i < children.Length; i++)
        {
            m_iconBox.Add(children[i]);
            children[i].gameObject.SetActive(false);
        }
        Initialize();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        for (int i = 0; i < m_iconBox.Count; i++)
        {
            m_iconBox[i].ActiveSwitch(false);
        }
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }

    /// <summary>
    /// アイコンの表示
    /// </summary>
    /// <param name="iconType"></param>
    public void IndicateIcon(IconType iconType)
    {
        for (int i = 0; i < m_iconBox.Count; i++)
        {
            if (m_iconBox[i].m_iconType == iconType)
            {
                m_iconBox[i].ActiveSwitch(true);
                break;
            }
        }
    }

    public void NotIndicateIcon(IconType iconType)
    {
        for (int i = 0; i < m_iconBox.Count; i++)
        {
            if (m_iconBox[i].m_iconType == iconType)
            {
                m_iconBox[i].ActiveSwitch(false);
                break;
            }
        }
    }

    public void ActiveSwitch(bool b)
    {
        gameObject.SetActive(b);
    }
}
