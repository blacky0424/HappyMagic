using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandButton : MonoBehaviour
{

    public Button m_hide;
    public Button m_feint;
    public Button m_camouflage;
    public Button m_listen;

    public List<GameObject> m_icon;
    float m_tansparency = 1.0f;

    List<Button> m_buttonBox = new List<Button>();

    [SerializeField]
    GameObject m_mousePrefab;
    CinderellaController m_cinderella;

    [SerializeField]
    Image m_camouFlageFade;

    [SerializeField, Header("かぼちゃのクールタイム")]
    float m_coolTime;
    [SerializeField]
    Image m_countFade;
    float m_coolTimer;
    public LayerMask layerMask = -1;
    MouseController mouseController;

    void Start()
    {
        m_buttonBox.Add(m_hide);
        m_buttonBox.Add(m_feint);
        m_buttonBox.Add(m_camouflage);
        m_buttonBox.Add(m_listen);
        
        m_camouFlageFade.enabled = false;

        m_cinderella = GameScene.Instance.Cinderella;
        m_countFade.fillAmount = 0f;

        GameObject obj = Instantiate(m_mousePrefab, Vector3.zero, Quaternion.identity);
        mouseController = obj.GetComponent<MouseController>();
        obj.SetActive(false);
    }

    void Update()
    {
        if (m_coolTimer > 0)
        {
            m_coolTimer -= Time.deltaTime;
            m_countFade.fillAmount = m_coolTimer / m_coolTime;
        }
        else
        {
            if (m_cinderella.gameObject.tag != TagName.Fake && m_coolTimer != 0)
            {
                CountFadeReset();
            }
        }

        if(m_cinderella.m_fakeTimer > 0)
        {
            m_camouFlageFade.enabled = true;
            m_camouFlageFade.fillAmount = 1 - (m_cinderella.m_fakeTimer / m_cinderella.m_fakeLimit);
        }
        else
        {
            if(m_cinderella.gameObject.tag != TagName.Fake && m_cinderella.m_fakeTimer == 0)
            {
                FakeFadeReset();
            }
        }    
    }

    /// <summary>
    /// コマンド聞き耳
    /// </summary>
    public void Listen()
    {
        Debug.Log("聞き耳");
        //範囲内のエネミーの位置を調べて保持
        List<EnemyBase> nearEnemys = GameScene.Instance.GetNearEnemys();
        //エネミーのコメント表示
        for (int i = 0; i < nearEnemys.Count; i++)
        {
            nearEnemys[i].ViewComent();
        }
        m_cinderella.Listen();
    }

    /// <summary>
    /// コマンド変装
    /// </summary>
    public void CamouFlage()
    {
        Debug.Log("変装");
        m_cinderella.m_fakeMode = true;
        SoundManager.Instance.PlaySE(SEName.Change);
        m_cinderella.ChangeCamouflage();
        ButtonNotActive();
    }

    /// <summary>
    /// コマンド陽動
    /// </summary>
    public void FeintMouse()
    {
        Vector3 mousePos;
        //シンデレラが向いている方向にネズミを生成させるための座標を変数に格納       
        Vector3 rayStart = new Vector3(m_cinderella.GetPosition.x, 1.0f, m_cinderella.GetPosition.z);
        Ray ray = new Ray(rayStart, m_cinderella.transform.forward);
        RaycastHit stageHit = new RaycastHit();
        //シンデレラの前方に壁があれば、ネズミを後方に生成
        if(Physics.Raycast(ray,out stageHit, 1.0f,layerMask))
        {
            mousePos = new Vector3(m_cinderella.GetPosition.x, 0.0f, m_cinderella.GetPosition.z) - (m_cinderella.transform.forward);
        }
        else
        {
            mousePos = new Vector3(m_cinderella.GetPosition.x, 0.0f, m_cinderella.GetPosition.z) + (m_cinderella.transform.forward);
        }
        SoundManager.Instance.PlaySE(SEName.Smoke);
        mouseController.Initialize(mousePos, m_cinderella.transform.rotation);        
        m_cinderella.IsFeintMode = true;
        ButtonNotActive();
        Debug.Log("陽動");
    }

    /// <summary>
    /// コマンド隠蔽
    /// </summary>
    public void Hide()
    {
        Debug.Log("隠蔽");
        m_cinderella.ChangePumpkin();
        ButtonNotActive();
    }

    /// <summary>
    /// クールタイム開始
    /// </summary>
    public void StartCoolTime()
    {
        m_coolTimer = m_coolTime;
        m_hide.interactable = false;
        m_countFade.fillAmount = 1f;
    }

    /// <summary>
    /// コマンドボタンが押せない状態へ
    /// </summary>
    public void ButtonNotActive()
    {
        m_tansparency = 0.3f;
        for(int i = 0;i < m_buttonBox.Count; i++)
        {
            m_buttonBox[i].interactable = false;
            m_icon[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, m_tansparency);
        }
    }

    /// <summary>
    /// コマンドボタンが押せる状態へ
    /// </summary>
    public void ButtonActive()
    {
        m_tansparency = 1.0f;
        for (int i = 0; i < m_buttonBox.Count; i++)
        {
            m_buttonBox[i].interactable = true;
            m_icon[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, m_tansparency);
        }
        if (m_coolTimer > 0 || m_coolTimer < 0)
        {
            m_hide.interactable = false;
        }
    }

    /// <summary>
    /// 隠蔽ボタン初期化
    /// </summary>
    public void CountFadeReset()
    {
        m_coolTimer = 0;
        m_hide.interactable = true;
        m_countFade.fillAmount = 0f;
    }
    
    /// <summary>
    /// 変装ボタン初期化
    /// </summary>
    public void FakeFadeReset()
    {
        m_camouFlageFade.fillAmount = 1f;
        m_camouFlageFade.enabled = false;
    }
}
