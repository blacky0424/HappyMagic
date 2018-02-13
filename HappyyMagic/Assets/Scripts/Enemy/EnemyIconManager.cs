using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 全てのアイコンを管理するクラス
/// </summary>
public class EnemyIconManager : MonoBehaviour {
    
    List<IconController> m_iconBox = new List<IconController>();
    GameObject m_silenceIcon;

    void Start() {
        //子オブジェクトの取得(上から順番)
        IconController[] children = GetComponentsInChildren<IconController>();
        for (int i = 0; i < children.Length;i++)
        {
            if (children[i].m_iconType == IconType.Silence)
            {
                m_silenceIcon = children[i].gameObject;
            }
            else
            {
                m_iconBox.Add(children[i]);
                children[i].gameObject.SetActive(false);
            }
        }
        Initialize();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        for(int i = 0;i < m_iconBox.Count; i++)
        {
            m_iconBox[i].gameObject.SetActive(false);
        }

        m_silenceIcon.SetActive(true);
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
        m_silenceIcon.SetActive(false);

        for (int i = 0; i < m_iconBox.Count; i++)
        {
            if(m_iconBox[i].m_iconType == iconType)
            {
                m_iconBox[i].ActiveSwitch(true);
            }
            else
            {
                //指定された以外のアイコンは非表示
                if (m_iconBox[i].gameObject.activeSelf)
                {
                    m_iconBox[i].ActiveSwitch(false);
                }
            }
        }
    }

    public void ActiveSwitch(bool b)
    {
        gameObject.SetActive(b);
    }
}
