using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearMovieScene : MonoBehaviour {

	void Start ()
    {
        Handheld.PlayFullScreenMovie("HappyMagic.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);

        SceneManager.Instance.LoadScene(SceneManager.ClearScene);
    }


}
