using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ResultScene : MonoBehaviour
{
    private Text ClearNotation;
    private Text ScatterDistance;
    private Text Degree;
    private Text FindTimes;
    private Text RemainingTime;
    private Text TotalScore;

    const string degree1 = "へっぴり腰";
    const string degree2 = "カチ割り初心者";
    const string degree3 = "思い切りのいい女";
    const string degree4 = "容赦なき者";
    const string degree5 = "ガラス割りのスペシャリスト";
    const string findString = "衛兵に見つかった回数: ";
    const string remaindString = "残り時間: ";
    const string totalScoreString = "総合スコア: ";

    bool SceneTransition;

    void Start()
    {
        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.FadeIn();
        }
        SoundManager.Instance.PlayBGM(BGMName.Result);

        SoundManager.Instance.PlaySE(SEName.GameClear);


        ClearNotation = GameObject.Find("ClearNotation").GetComponent<Text>();
        ScatterDistance = GameObject.Find("ScatterDistance").GetComponent<Text>();
        Degree = GameObject.Find("Degree").GetComponent<Text>();
        FindTimes = GameObject.Find("FindTimes").GetComponent<Text>();
        RemainingTime = GameObject.Find("RemainingTime").GetComponent<Text>();
        TotalScore = GameObject.Find("TotalScore").GetComponent<Text>();

        StartCoroutine(imagedisplay());

        ClearNotation.gameObject.SetActive(false);
        ScatterDistance.gameObject.SetActive(false);
        Degree.gameObject.SetActive(false);
        FindTimes.gameObject.SetActive(false);
        RemainingTime.gameObject.SetActive(false);
        TotalScore.gameObject.SetActive(false);

        SceneTransition = false;
    }

    void Update()
    {
        if (SceneTransition)
        {
            if (Input.GetMouseButtonDown(0))
            {
                FadeManager.Instance.FadeOut();
                SceneManager.Instance.LoadScene(SceneManager.TrueEndScene);
            }
        }
    }

    IEnumerator imagedisplay()
    {
        float breakPoint = ScoreManager.Instance.GrassBreak;
        ScatterDistance.text = "ガラスの破片 : " + breakPoint.ToString() + "m飛び散った";
        if (breakPoint >= 2.5f && breakPoint <= 3.0f)
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
        FindTimes.text = findString + ScoreManager.Instance.discovery + "回";
        RemainingTime.text = remaindString + ScoreManager.Instance.time.ToString() + "秒";
        ScoreManager.Instance.ScoreCalculation();
        TotalScore.text = totalScoreString + ScoreManager.Instance.Score.ToString() + "P";

        yield return new WaitForSeconds(1);
        ClearNotation.gameObject.SetActive(true);
        ScatterDistance.gameObject.SetActive(true);
        Degree.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        FindTimes.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        RemainingTime.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        TotalScore.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);

        SceneTransition = true;
    }

}
