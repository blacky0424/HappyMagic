using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class TutorialClearScene : MonoBehaviour {

    List<float> breakPointList = new List<float>();

    //一度のみ処理を実行するための変数
    bool once;
    float timer;

    // 加速度取得用変数
    public float breakPoint;

    float maxPower;

    // BreakTextが消えるまでの時間の設定
    float destroyTime = 3.0f;

    private Text breakText;

    void Start()
    {
        SoundManager.Instance.PlayBGM(BGMName.Treasure);

        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.FadeIn();
        }

        breakText = GameObject.Find("BreakText").GetComponent<Text>();
        once = false;
        timer = 0;

        breakText.gameObject.SetActive(true);
    }

    void Update()
    {
        // destroyTimer分の時間が経ったらBreakTextを消す
        timer += Time.deltaTime;
        if (timer > destroyTime)
        {
            breakText.gameObject.SetActive(false);
        }

        if (!once)
        {
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
                once = true;
                breakText.gameObject.SetActive(false);
            }
        }
                StartCoroutine(TutorialEnd());
    }

    IEnumerator TutorialEnd()
    {
        FadeManager.Instance.ChangeFadeColor(Color.white);
        yield return new WaitForSeconds(2);
        FadeManager.Instance.FadeOut();
        yield return new WaitForSeconds(1);
        SceneManager.Instance.LoadScene(SceneManager.ADV2Scene);
    }
}
