using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearMovieScene : MonoBehaviour {

	IEnumerator Start ()
    {
#if !UNITY_EDITOR
        yield return null;
#else
        yield return new WaitWhile(() => !Input.GetMouseButtonDown(0));
#endif

        Handheld.PlayFullScreenMovie("HappyMagic.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
        SceneManager.Instance.LoadScene(SceneManager.ClearScene);
    }


}
