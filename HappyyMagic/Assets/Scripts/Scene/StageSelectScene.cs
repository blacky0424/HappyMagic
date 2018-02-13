using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectScene : MonoBehaviour {

	void Start () {
        SoundManager.Instance.StopBGM();
        FadeManager.Instance.FadeIn();
        FadeManager.Instance.ChangeFadeColor(Color.black);
    }

    public void Stage1() {
        SceneManager.Instance.LoadScene(SceneManager.GameSceneStage1);
    }

    public void Stage2() {
        SceneManager.Instance.LoadScene(SceneManager.GameSceneStage2);
    }
}
