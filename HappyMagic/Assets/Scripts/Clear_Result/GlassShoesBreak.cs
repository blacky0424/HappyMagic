using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlassShoesBreak : MonoBehaviour
{
    List<float> breakPointList = new List<float>();

    private Vector3 Acceleration;
    private Vector3 preAcceleration;
    private Animator anim;
    float maxPower;

    GameObject HandwithGlass;
    GameObject Hand;
    GameObject GlassShoes;
    GameObject BreakedGlass;
                                                                                                           
    Rigidbody rb;

    // ローパスフィルター
    private float LowPassFilterFactor = (1.0f/60.0f) / 1.0f;
    private Vector3 lowPassValue = Vector3.zero;

    // 初期位置
    float lastY;

    // 加速度取得用変数
    public float breakPoint;

    // テキスト表示用変数
    public float BreakPoint;

    // 移動する時間の指定
    [SerializeField, Range(0, 10)]
    float time1 = 0.5f;

    // 振った後のオブジェクトの位置
    [SerializeField]
    Vector3 end1Position;
    
    private float startTime;
    private Vector3 startPosition;

    bool isThrew;

    bool isBreakSE;

    bool isAnimPlay;
 
    void OnEnable()
    {
        if(time1 <= 0)
        {
            transform.position = end1Position;
            enabled = false;
            return;
        }

        startTime = Time.timeSinceLevelLoad;
        startPosition = transform.position;

        isBreakSE = true;
    }

    void Start()
    {
        SoundManager.Instance.PlaySE(SEName.DestrFx);

        lowPassValue = Input.acceleration;　
        Input.gyro.enabled = true;
        this.HandwithGlass = gameObject;
        this.lastY = -0.3f;

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        Hand = GameObject.Find("Hand");
        BreakedGlass = GameObject.Find("BreakedGlass");
        GlassShoes = GameObject.Find("GlassShoes");

        Hand.SetActive(true);
        BreakedGlass.SetActive(false);
        GlassShoes.SetActive(true);

    }

    // ローパスフィルターで重力加速度の影響を取り除く
    Vector3 LowPassFilterAccelerometer()
    {
        lowPassValue = Vector3.Lerp(lowPassValue, Input.acceleration, LowPassFilterFactor);
        return lowPassValue;
    }

    void FixedUpdate()
    {
        // 端末が上下に動いた量
        float y = this.lastY + breakPoint;

        Vector3 lastPos = HandwithGlass.transform.position;
        Vector3 newPos = new Vector3(lastPos.x, y, lastPos.z);

        rb.MovePosition(newPos);
    }

    void Update()
    {

        breakPoint = Input.acceleration.magnitude;

        // 画面に触れている状態で端末を振った時にサンプルを取る
        if (Input.GetMouseButton(0) && breakPoint > 2.5f)
        {
            breakPointList.Add(breakPoint);
        }

        // 加速度のサンプルが5つ以上取れるまで次の処理は行わない
        if(breakPointList.Count < 5)
        {
            return;
        }

        maxPower = breakPointList.Max<float>();
        if (maxPower >= 2.5f)
        {
            isThrew = true;

            // 指定時間でオブジェクトを地面に移動
            var diff = Time.timeSinceLevelLoad - startTime;
            if (diff > time1)
            {
                transform.position = end1Position;
                enabled = false;
            }
            var rate = diff / time1;

            transform.position = Vector3.Lerp(startPosition, end1Position, rate);

            BreakPoint = Mathf.Round(maxPower * 10)/10;            

            if (end1Position.y == 0.12f)
            {
                Hand.SetActive(false);
                StartCoroutine(breakGlass());
            }
            
            ScoreManager.Instance.GrassBreak = BreakPoint;

            if (!isThrew)
            {
                return;
            }
        }
    }

    IEnumerator breakGlass()
    {
        yield return new WaitForSeconds(0.1f);
        if (isBreakSE)
        {
            SoundManager.Instance.PlaySE(SEName.Destruction123);
            isBreakSE = false;
        }
        BreakedGlass.SetActive(true);
        GlassShoes.SetActive(false);
        if (isAnimPlay)
        {
            anim.Play("glassBreak");
        }
        isAnimPlay = false;
    }

}
