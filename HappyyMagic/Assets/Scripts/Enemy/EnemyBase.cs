using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour
{
    protected const string IconBox = "IconBox";

    [SerializeField]
    protected EnemyProfile m_profile;
    [SerializeField]
    protected LayerMask m_layerMask = 4353;
    [SerializeField]
    protected List<Vector3> m_movePosList = new List<Vector3>(); //!<移動する位置のリスト

    protected Transform m_transform;
    protected NavMeshAgent m_navAgent;
    protected EnemyIconManager m_iconManager; //!< 吹き出し
    
    protected int m_arrivedCount; //!< 目的地に到着した回数  
    protected float m_distance; //!< シンデレラとの距離
    protected Vector3 basePos; //!< 初期位置

    //かぼちゃ
    protected bool m_pumpkin;
    protected float m_pumpkinTimer;

    protected bool isChase;

    protected bool m_isDicover;

    private int playerLayer;

    protected virtual void Start()
    {
        m_transform = GetComponent<Transform>();
        m_iconManager = m_transform.FindChild(IconBox).gameObject.GetComponent<EnemyIconManager>();
        m_navAgent = GetComponent<NavMeshAgent>();
        basePos = m_transform.position;
        m_pumpkin = false;
        m_pumpkinTimer = 0;

        m_isDicover = true;

        playerLayer = LayerMask.NameToLayer("Player");

        // ゲームマネージャに登録する
        GameScene.Instance.AddEnemy(this);
    }

    protected virtual void Update()
    {
#if UNITY_EDITOR
        //rayを飛ばす位置を修正
        Vector3 rayPos = new Vector3(m_transform.position.x, 1.0f, m_transform.position.z) - (m_transform.forward * m_profile.RayWide);
        Debug.DrawRay(rayPos, m_transform.forward * m_profile.RayDistance, Color.red);
#endif

        m_distance = Vector3.Distance(m_transform.position, GameScene.Instance.Cinderella.GetPosition);

        //シンデレラとの距離が近ければアイコン表示
        if (m_distance <= m_profile.IconRange && !m_iconManager.gameObject.activeSelf)
        {
            m_iconManager.ActiveSwitch(true);
        }
        //離れていれば非表示
        else if(m_distance > m_profile.IconRange && m_iconManager.gameObject.activeSelf)
        {
            m_iconManager.Initialize();
            m_iconManager.ActiveSwitch(false);
        }

        //かぼちゃを連続で発見できないように
        if (m_pumpkin)
        {
            m_pumpkinTimer += Time.deltaTime;
            if (m_pumpkinTimer > 6.0f)
            {
                m_pumpkin = false;
                m_pumpkinTimer = 0;
            }
        }

        if (!GameScene.Instance.Cinderella.IsRunning)
        {
            m_navAgent.speed = m_profile.WalkSpeed;
        }

        if (IsHeardFootstep())
        {
            m_navAgent.speed = m_profile.RunSpeed;
            SensingRunning();
        }
    }

    protected virtual GameObject SearchPlayerObject()
    {
        //rayを飛ばす位置を修正
        Vector3 rayPos = new Vector3(m_transform.position.x, 1.0f, m_transform.position.z) - (m_transform.forward * m_profile.RayWide);
        Ray ray = new Ray(rayPos, m_transform.forward);
        RaycastHit hit = new RaycastHit();
        //球形のrayを飛ばす
        if (Physics.SphereCast(ray, m_profile.RayWide, out hit, m_profile.RayDistance, m_layerMask))
        {
            return hit.transform.gameObject;
        }
        return null;
    }

    protected virtual bool IsHeardFootstep()
    {
        var cinderella = GameScene.Instance.Cinderella;
        if(cinderella.IsRunning && m_distance < m_profile.SensingRunRange)
        {
            if (isChase)
            {
                return true;
            }
            Vector3 rayPos = new Vector3(m_transform.position.x, 1.0f, m_transform.position.z);
            Ray ray = new Ray(rayPos, cinderella.GetPosition - m_transform.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, m_profile.RayDistance, m_layerMask))
            {
                return hit.transform.gameObject.layer == playerLayer;
            }
        }

        return false;
    }

    /// <summary>
    /// シンデレラの走行音を感知した時の処理（共通）
    /// </summary>
    protected virtual void SensingRunning()
    {
        //感知した座標に移動
        m_navAgent.destination = GameScene.Instance.Cinderella.GetPosition;
        if (!isChase)
        {
            m_iconManager.IndicateIcon(IconType.Exclamation);
        }
        isChase = true;
    }

    /// <summary>
    /// プレイヤー発見時にリスタートする
    /// </summary>
    public void Restart()
    {
        m_transform.position = basePos;
        m_navAgent.destination = basePos;
        m_arrivedCount = 0;
        m_iconManager.Initialize();
        m_isDicover = true;
    }

    public float GetIconRange
    {
        get
        {
            return m_profile.IconRange;
        }
    }

    /// <summary>
    /// アイコンやコメントの表示
    /// </summary>
    public abstract void ViewComent();


#if UNITY_EDITOR
    /// <summary>
    /// Rayを可視化
    /// </summary>
    void OnDrawGizmos()
    {
        if(m_profile == null)
        {
            LoadProfile();
        }

        Vector3 rayPos = new Vector3(transform.position.x, 1.0f, transform.position.z) - (transform.forward * m_profile.RayWide);
        Ray ray = new Ray(rayPos, transform.forward);
        RaycastHit hit = new RaycastHit();
        var isHit = Physics.SphereCast(ray, m_profile.RayWide, out hit, m_profile.RayDistance, m_layerMask);
        if (isHit)
        {
            Gizmos.DrawRay(rayPos, transform.forward * m_distance);
            Gizmos.DrawWireSphere(transform.position, m_profile.RayWide);
        }
    }

    void LoadProfile()
    {
        if (m_profile == null)
        {
            m_profile = UnityEditor.AssetDatabase.LoadAssetAtPath<EnemyProfile>("Assets/Resources/EnemyProfile.asset");

            if (m_profile == null)
            {
                var asset = ScriptableObject.CreateInstance<EnemyProfile>();
                UnityEditor.AssetDatabase.CreateAsset(asset, "Assets/Resources/EnemyProfile.asset");
                UnityEditor.AssetDatabase.Refresh();
                m_profile = UnityEditor.AssetDatabase.LoadAssetAtPath<EnemyProfile>("Assets/Resources/EnemyProfile.asset");
            }
        }
    }

    private void Reset()
    {
        LoadProfile();
    }
#endif //UNITY_EDITOR

}
