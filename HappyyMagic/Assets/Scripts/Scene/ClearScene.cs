using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearScene : MonoBehaviour {

    float Acceleration;

    float destroyTimer = 3.0f;
    float time = 0;

    private Text ClearNotation;
    private Text Degree;
    private Text ScatterDistance;
    private Text BreakText;
    const string degree1 = "へっぴり腰";
    const string degree2 = "カチ割り初心者";
    const string degree3 = "思い切りのいい女";
    const string degree4 = "容赦なき者";
    const string degree5 = "ガラス割りのスペシャリスト";

    bool SceneTransition;

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
        BreakText.gameObject.SetActive(true);

        SceneTransition = false;

    }

    void Update()
    {
        time += Time.deltaTime;
        if(time > destroyTimer)
        {
            Destroy(BreakText);
        }

        Acceleration = Input.acceleration.magnitude;
        if (Acceleration >= 2.5f)
        {
            Destroy(BreakText);
            StartCoroutine(imagedisplay());
        }

        if (SceneTransition)
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
        float breakPoint = ScoreManager.Instance.GrassBreak;
        ScatterDistance.text = "ガラスの破片 : " + breakPoint.ToString() + "m飛び散った";
 
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

        yield return new WaitForSeconds(1);
        ClearNotation.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        ScatterDistance.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        Degree.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);

        SceneTransition = true;

    }

}
