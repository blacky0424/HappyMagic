using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GameSceneの管理を行うクラス。
/// シンデレラの参照や敵の参照。制限時間などはここで管理
/// </summary>
public class GameScene : SingletonMonoBehaviour<GameScene>
{
    [SerializeField, Tooltip("1 = 1秒。Default = 300秒(5分)")]
    float m_limitTime = 300;

    float m_elapsedTime = 0.0f; // 経過時間
    bool m_isFinish;

    GameObject m_Lifecounter;
    int m_lifecount;

    public float elapsedtime {
        get { return m_elapsedTime; }
    }

    public int lifecount {
        get { return m_lifecount; }
    }

    public float LimitTime
    {
        get
        {
            return m_limitTime;
        }
    }

    CinderellaController m_cinderella;
    public List<EnemyBase> m_enemyList = new List<EnemyBase>();

    CommandButton m_commandButton;
    LifeCounter lifeCounter;

	void Start ()
    {
        m_cinderella = FindObjectOfType<CinderellaController>();
        m_commandButton = FindObjectOfType<CommandButton>();
        m_Lifecounter = GameObject.Find("LifeCounter");

        lifeCounter = m_Lifecounter.GetComponent<LifeCounter>();

        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.FadeIn();
        }
        SoundManager.Instance.StopBGM();
        Invoke( ((System.Action)LatePlayBGM).Method.Name, 1.0f);
    }

    void LatePlayBGM()
    {
        SoundManager.Instance.PlayBGM(BGMName.Gameplay);
    }

    void Update ()
    {
        if(!m_isFinish && m_elapsedTime > m_limitTime)
        {
            m_isFinish = true;
            SoundManager.instance.PlaySE(SEName.Bell);
            // TODO: 時間切れのフラグを立てる

            Debug.Log("ゲームオーバーです。");
            Invoke((((System.Action)LoadGameOverScene).Method.Name), 3.0f);
        }


        if (!m_isFinish)
        {
            LifeCheck();
        }

        m_elapsedTime += Time.deltaTime;
	}

    /// <summary>
    /// 敵をリストに追加する
    /// </summary>
    /// <param name="enemy"></param>
    public void AddEnemy(EnemyBase enemy)
    {
        m_enemyList.Add(enemy);
    }

    /// <summary>
    /// 渡された座標から指定された範囲内で一番近くにいる敵を返す。
    /// いなければnullを返す。
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public EnemyBase GetNearEnemy(Vector3 position, float range)
    {
        // TODO: 一番近い敵を探す
        // TODO: その位置を返す

        return m_enemyList[0];
    }

    /// <summary>
    /// 渡された座標から指定された範囲内にいる敵を全て返す(必要があれば数の指定、近い順にソートする)
    /// </summary>
    /// <returns></returns>
    public List<EnemyBase> GetNearEnemys()
    {
        Vector3 cinderellaPos = Cinderella.transform.position;
        List<EnemyBase> nearEnemys = new List<EnemyBase>();
        // 範囲内の敵をすべて探す
        for (int i = 0; i < m_enemyList.Count; i++)
        {
            float distance = (cinderellaPos - m_enemyList[i].transform.position).magnitude;
            if (distance < m_enemyList[i].GetIconRange)
            {
                nearEnemys.Add(m_enemyList[i]);
            }
        }

        // TODO: 近い順に並べる。
        // TODO: 指定された数だけをListに追加する

        // TODO: 追加されたリストを返す
        return nearEnemys;
    }

    public List<EnemyBase> EnemyList
    {
        get
        {
            return m_enemyList;
        }
    }

    public CinderellaController Cinderella
    {
        get
        {
            return m_cinderella;
        }
    }

    public CommandButton CommandBotton
    {
        get
        {
            return m_commandButton;
        }
    }

    public LifeCounter LifeCounter
    {
        get
        {
            return lifeCounter;
        }
    }

    void LifeCheck()
    {
        m_lifecount = lifeCounter.LifeCount;
        if (m_lifecount <= 0) {
            //HP切れのフラグを立てる
            m_elapsedTime = m_limitTime;
        }

    }

    void LoadGameOverScene()
    {
        // シーンの読み込み
        SceneManager.Instance.LoadScene(SceneManager.GameOverScene);
    }

}
