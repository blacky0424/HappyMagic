using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectScene : MonoBehaviour {

	void Start () {
        SceneManager.Instance.LoadScene(SceneManager.TutorialClearScene);
    }
	
}
