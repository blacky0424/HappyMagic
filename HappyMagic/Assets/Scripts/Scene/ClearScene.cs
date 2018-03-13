using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ClearScene : MonoBehaviour {

    List<float> breakPointList = new List<float>();

    // BreakTextが消えるまでの時間の設定
    float destroyTimer = 3.0f;
    float time = 0;

    // 加速度取得用変数
    public float breakPoint;

    float maxPower;

    private Text ClearNotation;
    private Text Degree;
    private Text ScatterDistance;
    private Text BreakText;
    const string degree1 = "へっぴり腰";
    const string degree2 = "カチ割り初心者";
    const string degree3 = "思い切りのいい女";
    const string degree4 = "容赦なき者";
    const string degree5 = "ガラス割りのスペシャリスト";

    bool sceneTransition;

    void Start()
    {
        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.FadeIn();
        }

        SoundManager.Instance.PlayBGM(BGMName.Treasure);

        ClearNotation = GameObject.Find("ClearNotation").GetComponent<Text>();
        ScatterDistance = GameObject.Find("ScatterDistance").GetComponent<Text>();
        Degree = GameObject.Find("Degree").GetComponent<Text>();
        BreakText = GameObject.Find("BreakText").GetComponent<Text>();

        ClearNotation.gameObject.SetActive(false);
        ScatterDistance.gameObject.SetActive(false);
        Degree.gameObject.SetActive(false);

        sceneTransition = false;

    }

    void Update()
    {
        // destroyTimer分の時間が経ったらBreakTextを消去
        time += Time.deltaTime;
        if(time > destroyTimer)
        {
            Destroy(BreakText);
        }

        breakPoint = Input.acceleration.magnitude;

        // 画面に触れている状態で端末を振った時にサンプルを取る
        if (Input.GetMouseButton(0) && breakPoint > 2.5f)
        {
            breakPointList.Add(breakPoint);
        }

        // 加速度のサンプルが5つ以上取れるまで次の処理は行わない
        if (breakPointList.Count < 5)
        {
            return;
        }

        maxPower = breakPointList.Max<float>();

        if (maxPower >= 2.5f)
        {
            Destroy(BreakText);
        }

        StartCoroutine(imagedisplay());

        // sceneTransitionがtrueなら画面タッチでシーン遷移
        if (sceneTransition)
        {
            if (Input.GetMouseButtonDown(0))
            {
                FadeManager.Instance.FadeOut();
                SceneManager.Instance.LoadScene(SceneManager.ResultScene);
            }
        }

    }

    IEnumerator imagedisplay()
    {
        breakPoint = ScoreManager.Instance.GrassBreak;
        ScatterDistance.text = "ガラスの破片 : " + breakPoint.ToString() + "m飛び散った";
        
        // 振った強さ(加速度センサーの大きさ)によって称号を変える
        if(breakPoint >= 2.5f && breakPoint <= 3.0f)
        {   
            Degree.text = degree1;
        }
        else if (breakPoint > 3.0f && breakPoint <= 3.5f)
        {
            Degree.text = degree2;
        }
        else if (breakPoint > 3.5f && breakPoint <= 4.0f)
        {
            Degree.text = degree3;
        }
        else if (breakPoint > 4.0f && breakPoint <= 4.5f)
        {
            Degree.text = degree4;
        }
        else if (breakPoint > 4.5f)
        {
            Degree.text = degree5;
        }

        // Textを順に表示
        yield return new WaitForSeconds(1);
        ClearNotation.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        ScatterDistance.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        Degree.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        sceneTransition = true;

    }

}
