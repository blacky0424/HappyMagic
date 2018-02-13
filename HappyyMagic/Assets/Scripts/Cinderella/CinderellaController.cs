using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シンデレラの挙動を管理するクラス
/// </summary>
public class CinderellaController : MonoBehaviour
{
    [SerializeField]
    GameObject cinderella;
    [SerializeField]
    GameObject pumpkin;
    [SerializeField]
    GameObject fakeMercenary;

    [SerializeField]
    CinderellaWandViewer m_wandViewer;
    [SerializeField]
    GameObject m_fakeParticle;
    [SerializeField]
    GameObject m_pumpkinParticle;
    [SerializeField]
    GameObject m_pumpkinReleaseParticle;

    [SerializeField]
    float m_moveSpeed;
    [SerializeField, Header("歩行速度")]
    float m_walkSpeed;
    [SerializeField, Header("走行速度")]
    float m_runSpeed;

    //衛兵に変身できる時間
    public float m_fakeLimit;
    [HideInInspector]
    public float m_fakeTimer;
    public bool m_fakeMode;
    //変身中
    bool m_isChange;

    public bool IsRunning { get; private set; }
    //陽動
    public bool IsFeintMode { get; set; }

    //アイコン
    CinderellaIconManager m_iconManager;

    Rigidbody m_rigidbody;
    CinderellaAnimator m_animator;
    Transform m_camera;

    Vector3 startPos;
    Quaternion startRot;

    void Start ()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_animator = new CinderellaAnimator(GetComponentsInChildren<Animator>(true));
        m_camera = FindObjectOfType<CinderellaCamera>().GetComponent<Transform>();
        m_fakeMode = false;
        IsFeintMode = false;
        m_iconManager = transform.FindChild("IconBox").gameObject.GetComponent<CinderellaIconManager>();

        m_wandViewer.OnHideWandObj();

        startPos = transform.position;
        startRot = transform.rotation;

        pumpkin.SetActive(false);
        fakeMercenary.SetActive(false);
    }

    private void Update()
    {
        //タイマー
        if (this.tag == TagName.Fake)
        {
            m_fakeTimer += Time.deltaTime;
        }
        //変身できる時間を超えたら元のタグに
        if (m_fakeTimer > m_fakeLimit)
        {
            m_fakeMode = false;
            this.tag = TagName.Player;
            ChangeModel(fakeMercenary, cinderella);
            m_fakeTimer = 0;
            GameScene.Instance.CommandBotton.ButtonActive();
        }
    }

    void FixedUpdate () {

        float h = GamePadManager.Instance.GetAxis(GamePadAxis.Horizontal);
        float v = GamePadManager.Instance.GetAxis(GamePadAxis.Vertical);


        Vector3 move = new Vector3(h, 0.0f, v);
        // 入力がなければ何もしない
        if (move != Vector3.zero)
        {
            //スティックの引っ張り具合により速度が変わる
            if(h > 0.5f || h < -0.5f || v > 0.5f || v < -0.5f)
            {
                m_moveSpeed = m_runSpeed;
            }
            else
            {
                m_moveSpeed = m_walkSpeed;
            }

            if (this.tag != TagName.Fake && !m_isChange)
            {
                if (h > 0.5f || h < -0.5f || v > 0.5f || v < -0.5f)
                {
                    //走行中
                    IsRunning = true;
                }
                else
                {
                    IsRunning = false;
                }
            }
            else
            {
                //変装中は走行不可
                m_moveSpeed = m_walkSpeed;
            }

            if (this.tag == TagName.Pumpkin)
            {
                //スティックを動かしたら戻る
                SoundManager.Instance.PlaySE(SEName.Smoke);
                SoundManager.Instance.PlaySE(SEName.Landing);
                m_pumpkinReleaseParticle.SetActive(true);
                this.tag = TagName.Player;
                ChangeModel(pumpkin, cinderella);
                GameScene.Instance.CommandBotton.ButtonActive();
                GameScene.Instance.CommandBotton.StartCoolTime();
            }

            if (IsFeintMode || m_isChange)
            {
                //コマンド発動からネズミを出撃させるまでもしくは変身中は移動不可
                m_moveSpeed = 0f;
            }

            var cameraForward = Vector3.Scale(m_camera.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 direction = cameraForward * v + m_camera.right * h;

            //移動(物理を適用したオブジェクトはFixedUpdateで動かさないといけない)
            m_rigidbody.MovePosition(transform.position + direction * m_moveSpeed * Time.fixedDeltaTime);
            m_rigidbody.MoveRotation(Quaternion.LookRotation(direction));
        }
        else
        {
            IsRunning = false;
        }

        AnimatorUpdate(Mathf.Abs(h), Mathf.Abs(v));
    }

    void AnimatorUpdate(float h, float v)
    {
        // Animatorの更新
        m_animator.MoveSpeed = h > v ? h : v;
    }

    public void ChangeCamouflage()
    {
        IsRunning = false;
        m_isChange = true;
        m_fakeParticle.SetActive(true);
        m_animator.SetDoChange();
        StartCoroutine(WaitChangeAnimation(() => 
        {
            m_isChange = false;
            gameObject.tag = TagName.Fake;
            ChangeModel(cinderella, fakeMercenary);
            m_wandViewer.OnHideWandObj();
        }));
    }

    public void ChangePumpkin()
    {
        m_isChange = true;
        m_animator.SetDoPumpkin();
        StartCoroutine(WaitChangeAnimation( ()=>
        {
            m_pumpkinParticle.SetActive(true);
            m_isChange = false;
            gameObject.tag = TagName.Pumpkin;
            ChangeModel(cinderella, pumpkin);
            m_wandViewer.OnHideWandObj();
        }));
    }

    public void MouseAnim()
    {
        m_animator.SetDoMouseSet();
    }

    public void ReleaseFeintMode()
    {
        IsFeintMode = false;
    }

    /// <summary>
    /// モデルの変更
    /// </summary>
    /// <param name="currentModel"></param>
    /// <param name="changeModel"></param>
    void ChangeModel(GameObject currentModel, GameObject changeModel)
    {
        currentModel.SetActive(false);
        changeModel.SetActive(true);
    }

    IEnumerator WaitChangeAnimation(System.Action callback)
    {
        yield return null;
        yield return new WaitForSeconds(0.25f);
        yield return new WaitForAnimation(m_animator.ActiveComponent, 0);
        callback();
    }

    /// <summary>
    /// 傭兵からの発見された時リスタート
    /// </summary>
    public void Restart()
    {
        transform.position = startPos;
        transform.rotation = startRot;

        m_iconManager.Initialize();
        this.tag = TagName.Player;
        GameScene.Instance.CommandBotton.CountFadeReset();
        IsFeintMode = false;
        GameScene.Instance.CommandBotton.FakeFadeReset();
        m_fakeMode = false;
        m_fakeTimer = 0;

        pumpkin.SetActive(false);
        fakeMercenary.SetActive(false);
        cinderella.SetActive(true);
    }

    /// <summary>
    /// ストーキングアイコン表示
    /// </summary>
    public void IsStalking()
    {
        m_iconManager.IndicateIcon(IconType.Stalking);
    }

    /// <summary>
    /// ストーキングアイコン非表示
    /// </summary>
    public void NotStalking()
    {
        m_iconManager.NotIndicateIcon(IconType.Stalking);
    }

    /// <summary>
    /// 聞き耳アイコン表示
    /// </summary>
    public void Listen()
    {
        m_iconManager.IndicateIcon(IconType.Listen);
    }

    /// <summary>
    /// ！アイコン表示
    /// </summary>
    public void Exclamation()
    {
        m_iconManager.IndicateIcon(IconType.Exclamation);
    }

    public Vector3 GetPosition
    {
        get
        {
            return transform.position;
        }
    }

    public float GetAnimationTime
    {
        get
        {
            return m_animator.ActiveComponent.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
    }
}
