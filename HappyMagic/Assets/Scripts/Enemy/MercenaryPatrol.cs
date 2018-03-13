using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 見回りを行う傭兵の制御クラス
/// </summary>
public class MercenaryPatrol : EnemyBase
{

    float m_elapsedTime;

    MercenaryAnimator m_animator;

    protected override void Start()
    {
        base.Start();

        m_animator = new MercenaryAnimator(GetComponentInChildren<Animator>());

        m_arrivedCount = 0;
        m_navAgent.SetDestination(m_movePosList[m_arrivedCount]);
        m_animator.SetDoWalk();
    }

    protected override void Update()
    {
        //Enemyの更新
        base.Update();

        if (m_navAgent.speed == m_profile.RunSpeed)
        {
            int anim = m_animator.CurrentStateHash;
            //走行アニメーションになっていなければ
            if (anim != m_animator.GetStateHash(MercenaryAnimator.StateName.Run))
            {
                //走行アニメーション
                m_animator.SetDoRun();
            }
        }

        //シンデレラとの距離が近ければ探す
        if (m_distance < 20.0f)
        {
            SearchPlayer();
        }

        //目的地に移動完了したら立ち止まる
        if (m_transform.position.x.SafeEquals(m_navAgent.destination.x) &&
            m_transform.position.z.SafeEquals(m_navAgent.destination.z))
        {
            //待機アニメーション
            if (m_elapsedTime == 0)
            {
                m_animator.SetDoStop();
            }
            m_elapsedTime += Time.deltaTime;
            m_navAgent.speed = 0;
        }

        //次の目的地を決める
        if (m_elapsedTime > m_profile.MoveWaitTime)
        {
            m_arrivedCount++;
            //進行パターンによって変化

            //一巡したら初期化
            if (m_arrivedCount >= m_movePosList.Count)
            {
                m_arrivedCount = 0;
            }
            m_navAgent.SetDestination(m_movePosList[m_arrivedCount]);
            //歩行アニメーション
            m_animator.SetDoWalk();
            m_elapsedTime = 0;
            if (isChase)
            {
                isChase = false;
            }
        }
    }

    /// <summary>
    /// シンデレラの走行音を感知した時の処理（タイプ別）
    /// </summary>
    protected override void SensingRunning()
    {
        base.SensingRunning();
    }

    public override void ViewComent()
    {
        m_iconManager.IndicateIcon(IconType.Patrol);
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
            var LifeCountScript = GameScene.Instance.LifeCounter;
            LifeCountScript.lifeDamage();
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
        m_animator.SetDoReactionPumpkin();

        yield return null;

        yield return new WaitForAnimation(m_animator.Component, 0);

        yield return new WaitForSeconds(2.0f);

        m_iconManager.IndicateIcon(IconType.HideSuccess);
    }


}
