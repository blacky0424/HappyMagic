using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイコンの種類
/// </summary>
public enum IconType
{
    Concentration,  //集中
    Distracted,     //注意力散漫
    Patrol,         //巡回
    Notice,         //発見（マウス、パンプキン）
    Stalking,       //ストーキング中
    HideSuccess,    //隠蔽成功
    MouseSuccess,   //陽動成功
    Listen,         //聞き耳
    Exclamation,    //！

    Silence,        //沈黙(・・・) <- 初期状態
}

/// <summary>
/// 個別にアイコンを管理するクラス
/// </summary>
public class IconController : MonoBehaviour
{
    public IconType m_iconType;

    float m_timer;
    //表示時間
    [SerializeField]
    float m_displayTime;

    public void ActiveSwitch(bool b)
    {
        gameObject.SetActive(b);
        m_timer = 0;
    }

    void Update()
    {
        //タイプ別の更新
        switch (m_iconType) {
            case IconType.Concentration:
            case IconType.Distracted:
            case IconType.Patrol:
            case IconType.Notice:
            case IconType.HideSuccess:
            case IconType.MouseSuccess:
            case IconType.Exclamation:
            case IconType.Listen:
                if (m_timer >= 0)
                {
                    m_timer += Time.deltaTime;
                }
                if (m_timer > m_displayTime)
                {
                    m_timer = 0;
                    ActiveSwitch(false);
                }
                break;
        }
    }
}
