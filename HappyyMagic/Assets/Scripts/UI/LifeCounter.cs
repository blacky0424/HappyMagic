using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCounter : MonoBehaviour
{
    int m_LifeCount;
    public List<GameObject> Lifes;
    public int LifeCount
    {
        get
        {
            return m_LifeCount + 1;
        }
    }

    public GameObject Cinderella;
    bool m_Damege;
    bool m_Damegenow;
    public int m_DamegeTime = 100;

    TutorialScene t;

    // Use this for initialization
    void Start()
    {
        m_LifeCount = Lifes.Count - 1;

        //チュートリアルかどうかの確認
        t = FindObjectOfType<TutorialScene>();
    }

    // Update is called once per frame
    void Update()
    {
        DamegeCheck();

    }

    public void lifeDamage()
    {
        //本編
        if (t == null)
        {
            if (m_LifeCount < 0)
            {
                return;
            }
            else if (!m_Damege)
            {
                Lifes[m_LifeCount].SetActive(false);
                m_LifeCount--;
                m_Damege = true;
                Debug.Log(LifeCount);
            }
        }
        //チュートリアル
        else
        {
            m_Damege = true;
        }
    }

    IEnumerator DamageTime()
    {
        yield return new WaitForSeconds(3.0f);

        //マウスが出ている場合すべて削除
        MouseController[] mouses = FindObjectsOfType<MouseController>();
        if (mouses.Length != 0)
        {
            for (int i = 0; i < mouses.Length; i++)
            {
                mouses[i].MouseDisabled();
            }
        }
        //それぞれ初期化
        GameScene.Instance.CommandBotton.ButtonActive();
        GameScene.Instance.Cinderella.Restart();
        if (t != null) {
            t.Restart();
        }
        for (int i = 0; i < GameScene.Instance.EnemyList.Count; i++)
        {
            GameScene.Instance.EnemyList[i].Restart();
        }

        FadeManager.Instance.FadeIn(()=> 
        {
            m_Damege = false;
            m_Damegenow = false;
        });
    }

    void DamegeCheck()
    {
        if (m_Damege&&!m_Damegenow)
        {
            FadeManager.Instance.FadeOut(()=> 
            {
                m_Damegenow = true;

                //HPがあればリスタートの準備に
                if (m_LifeCount >= 0)
                {
                    StartCoroutine(DamageTime());
                }
            });
        }
            
        
    }

}
