using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 注意力散漫の傭兵の制御クラス
/// </summary>
public class MercenaryDistracted : EnemyBase
{
    //向かない方向 (下360 : 0 左270: 3 上180 : 2 右90 : 1)　注意力散漫用
    [SerializeField, Header("注意力散漫用")]
    private float[] notDirection;

    float m_elapsedTime;
    Vector3 m_nextDir;

    MercenaryAnimator m_animator;

    protected override void Start ()
    {
        base.Start();

        m_animator = new MercenaryAnimator(GetComponentInChildren<Animator>());
        
        basePos = m_transform.position;
        m_nextDir = new Vector3(0, -90, 0);

        m_navAgent.destination = basePos;
        m_animator.SetDoStop();
    }
	
	protected override void Update ()
    {
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
        if (m_distance < m_profile.SensingRange)
        {
            SearchPlayer();
        }

        //初期位置にいるとき
        if (m_transform.position.x.SafeEquals(basePos.x) &&
           m_transform.position.z.SafeEquals(basePos.z))
        {
            int anim = m_animator.CurrentStateHash;
            if (!m_pumpkin)
            {
                //待機アニメーションになっていなければ
                if (anim != m_animator.GetStateHash(MercenaryAnimator.StateName.Idle))
                {
                    //待機アニメーション
                    m_animator.SetDoStop();
                    isChase = false;
                }
            }
            if (m_elapsedTime > m_profile.MoveWaitTime)
            {
                //回転角度の更新
                m_nextDir -= new Vector3(0, 90, 0);
                //更新角度が見ない方向だったらさらに更新
                for (int i = 0; i < notDirection.Length; i++)
                {
                    if ((Mathf.Abs((m_nextDir.y / 90.0f)) % 4).SafeEquals(notDirection[i]))
                    {
                        m_nextDir -= new Vector3(0, 90, 0);
                    }
                }

                m_elapsedTime = 0;
            }
            //回転中
            if (Mathf.DeltaAngle(m_transform.eulerAngles.y, m_nextDir.y) < -3.0f ||
                Mathf.DeltaAngle(m_transform.eulerAngles.y, m_nextDir.y) > 3.0f)
            {
                m_transform.Rotate(new Vector3(0f, -5.0f, 0f));
            }
            //回転終了
            else
            {
                m_elapsedTime += Time.deltaTime;
            }
        }
        else
        {
            //目的地到着後
            if (m_transform.position.x.SafeEquals(m_navAgent.destination.x) &&
               m_transform.position.z.SafeEquals(m_navAgent.destination.z))
            {
                m_elapsedTime += Time.deltaTime;
                if (isChase)
                {
                    isChase = false;
                }
            }

            if(m_elapsedTime > m_profile.MoveWaitTime)
            {
                m_elapsedTime = 0;
                //初期位置に戻る
                m_navAgent.destination = basePos;
                m_animator.SetDoWalk();
            }
        }

    }

    protected override void SensingRunning()
    {
        base.SensingRunning();
        m_elapsedTime = 0;
    }

    public override void ViewComent()
    {
        m_iconManager.IndicateIcon(IconType.Distracted);
    }

    /// <summary>
    /// シンデレラを探す
    /// </summary>
    void SearchPlayer()
    {
        GameObject hitObject = SearchPlayerObject();
        if(hitObject == null)
        {
            return;
        }
        
        if (hitObject.tag == TagName.Player)
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
        else if (hitObject.tag == TagName.Fake)
        {
            m_animator.SetDoWalk();
            m_navAgent.destination = basePos;
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
