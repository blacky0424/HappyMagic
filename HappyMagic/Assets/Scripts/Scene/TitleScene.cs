using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
	
    void Start()
    {
        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.FadeIn();
        }

        SoundManager.Instance.PlayBGM(BGMName.Title);
    }

	void Update () {

	}

    public void OnGameStart()
    {
        SceneManager.Instance.LoadScene(SceneManager.ADVScene);
    }
}
