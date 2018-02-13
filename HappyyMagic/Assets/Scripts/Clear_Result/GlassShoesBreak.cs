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

    private float LowPassFilterFactor = (1.0f/60.0f) / 1.0f;
    private Vector3 lowPassValue = Vector3.zero;

    float lastY;
    // 加速度取得用変数
    public float breakPoint;

    // テキスト表示用
    public float BreakPoint;

    [SerializeField, Range(0, 10)]
    float time1 = 0.5f;

    [SerializeField]
    Vector3 end1Position;
    
    private float startTime;
    private Vector3 startPosition;

    bool isThrew;
 
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

        float maxPower = breakPointList.Max<float>();
        if (maxPower >= 2.5f)
        {
            isThrew = true;

            var diff = Time.timeSinceLevelLoad - startTime;
            if (diff > time1)
            {
                transform.position = end1Position;
                enabled = false;
            }
            var rate = diff / time1;

            transform.position = Vector3.Lerp(startPosition, end1Position, rate);

            BreakPoint = Mathf.Round(maxPower * 10)/10;
            Debug.Log("Acceleration: " + BreakPoint);

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
        SoundManager.Instance.PlaySE(SEName.Destruction123);
        BreakedGlass.SetActive(true);
        GlassShoes.SetActive(false);
        anim.Play("glassBreak");
    }

}
