using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialClearScene : MonoBehaviour {

    bool b;

    void Start()
    {
        SoundManager.Instance.PlayBGM(BGMName.Treasure);
        SoundManager.Instance.PlaySE(SEName.DestrFx);
        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.FadeIn();
        }

        b = false;
    }

    void Update()
    {
        if (!b)
        {
            float g = Input.acceleration.magnitude;
            if (g >= 2.5f)
            {
                b = true;
                StartCoroutine(TutorialEnd());
            }
        }
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
