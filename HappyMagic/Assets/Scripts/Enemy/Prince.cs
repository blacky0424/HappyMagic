using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 王子の挙動管理クラス
/// </summary>
public class Prince : EnemyBase
{

    float m_elapsedTime;
    [SerializeField]
    float m_stakingRange;
    //王子のストーキングポイント
    float m_stalkingPoint;
    [SerializeField]
    float m_stalkingPointMax;
    //メモイベント
    bool m_memoFinish;
    [SerializeField]
    GameObject m_memo;

    //止まる場所の指定
    [SerializeField]
    int[] m_stopPosNum;
    //指定した場所で止まる時間
    [SerializeField]
    float m_stopPosWaitTime = 2.0f;
    //元の止まる時間
    float m_originWaitTime;
    float m_waitTime;

    //プレイヤー発見時
    float m_loadTimer;
    bool m_playerFound;

    PrinceAnimator m_animator;

    protected override void Start()
    {
        base.Start();

        m_animator = new PrinceAnimator(GetComponentInChildren<Animator>());

        m_arrivedCount = 0;
        m_originWaitTime = m_profile.MoveWaitTime;

        //最初の目的地を決定
        m_navAgent.SetDestination(m_movePosList[m_arrivedCount]);
        m_animator.SetDoWalk();
        m_iconManager = transform.FindChild(IconBox).gameObject.GetComponent<EnemyIconManager>();
    }

    protected override void Update()
    {
        //Enemyの更新
        base.Update();
        //現在のアニメ―ションの状態を取得
        int anim = m_animator.CurrentStateHash;
        if (m_navAgent.speed.SafeEquals(m_profile.RunSpeed))
        {
            //走行アニメーションになっていなければ
            if (anim != m_animator.GetStateHash(PrinceAnimator.StateName.Run))
            {
                //走行アニメーション
                m_animator.SetDoRun();
            }
        }

        //目的地に移動完了したら立ち止まる
        if (transform.position.x.SafeEquals(m_navAgent.destination.x) &&
        transform.position.z.SafeEquals(m_navAgent.destination.z))
        {
            //待機アニメーションになっていなければ
            if (m_elapsedTime == 0 && !m_pumpkin)
            {
                //待機アニメーション
                m_animator.SetDoStop();
            }

            m_elapsedTime += Time.deltaTime;
        }
        if (m_elapsedTime > m_waitTime)
        {
            m_elapsedTime = 0;
            m_arrivedCount++;
            //一巡したら初期化
            if (m_arrivedCount >= m_movePosList.Count)
            {
                m_arrivedCount = 0;
            }

            if (isChase)
            {
                isChase = false;
            }

            //止まる時間の変更
            m_waitTime = m_originWaitTime;
            for(int i = 0;i < m_stopPosNum.Length; i++)
            {
                if (m_arrivedCount == m_stopPosNum[i])
                {
                    m_waitTime = m_stopPosWaitTime;
                    break;
                }
            }
            //歩行アニメーション
            m_animator.SetDoWalk();

            m_navAgent.SetDestination(m_movePosList[m_arrivedCount]);
        }

        //シンデレラと距離が近ければ探す
        if (m_distance < m_profile.SensingRange)
        {
            SearchPlayer();
            //ストーキング判定
            if (m_distance < m_stakingRange && !m_memoFinish && GameScene.Instance.Cinderella.tag == TagName.Player)
            {
                IsStalking();
                GameScene.Instance.Cinderella.IsStalking();
            }
            else
            {
                GameScene.Instance.Cinderella.NotStalking();
            }
        }

        //ストーキングポイントが上限値を超えたら
        if (m_stalkingPoint > m_stalkingPointMax)
        {
            m_stalkingPoint = m_stalkingPointMax;
            MemoEvent();
        }

        if (m_playerFound)
        {
            //チュートリアルかどうかの確認
            TutorialScene t = FindObjectOfType<TutorialScene>();
            m_playerFound = false;
            //本編
            if (t == null)
            {
                //プレイヤー発見時、専用のシーンへ移行
                m_loadTimer += Time.deltaTime;
                if (m_loadTimer > 2.0f)
                {
                    SceneManager.Instance.LoadScene(SceneManager.PrinceEndScene);
                }
            }
            //チュートリアル
            else
            {
                GameScene.Instance.LifeCounter.lifeDamage();
            }

        }
    }


    void SearchPlayer()
    {
        GameObject hitObject = SearchPlayerObject();
        if(hitObject == null)
        {
            return;
        }

        if (hitObject.tag == TagName.Player || hitObject.tag == TagName.Fake)
        {
            if (base.m_isDicover)
            {
                SoundManager.Instance.PlaySE(SEName.Discover);
                base.m_isDicover = false;
            }
            m_iconManager.IndicateIcon(IconType.Exclamation);
            GameScene.Instance.Cinderella.Exclamation();
            m_animator.SetDoDiscover();
            m_playerFound = true;
            FadeManager.Instance.FadeOut();
        }
        else if (hitObject.tag == TagName.Pumpkin)
        {
            if (!m_pumpkin)
            {
                StartCoroutine(PumpkinAnim());
            }
        }

    }

    IEnumerator PumpkinAnim()
    {
        m_iconManager.IndicateIcon(IconType.Notice);
        m_elapsedTime = 0;
        m_navAgent.destination = transform.position;
        m_pumpkin = true;
        m_animator.SetDoDiscover();

        yield return null;

        yield return new WaitForAnimation(m_animator.Component, 0);

        yield return new WaitForSeconds(2.0f);

        m_navAgent.destination = transform.position;
        m_animator.SetDoReactionPumpkin();
        m_iconManager.IndicateIcon(IconType.HideSuccess);

    }

    /// <summary>
    /// シンデレラの走行音を感知した時の処理（タイプ別）
    /// </summary>
    protected override void SensingRunning()
    {
        base.SensingRunning();
    }

    void IsStalking()
    {
        m_stalkingPoint += Time.deltaTime;
    }

    /// <summary>
    /// 王子ストーキング成功によるメモイベント
    /// </summary>
    void MemoEvent()
    {
        m_memoFinish = true;
        m_memo.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseMemo()
    {
        m_memo.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public override void ViewComent()
    {

    }
}
