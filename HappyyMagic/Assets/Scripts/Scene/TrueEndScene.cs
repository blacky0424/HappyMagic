using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueEndScene : MonoBehaviour {

	void Start () {
        FadeManager.Instance.FadeIn();
	}
	
	void Update () {
		
	}

    public void LoadTitleScene()
    {
        FadeManager.Instance.FadeOut();
        SceneManager.Instance.LoadScene(SceneManager.TitleScene);
    }
}
