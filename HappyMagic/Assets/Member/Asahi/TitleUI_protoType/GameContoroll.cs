using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContoroll : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        sceneChange();
	}

    void sceneChange() {
        //touch判定
        if (!Input.GetMouseButtonDown(0)) return;
        //Scene移動
        SceneManager.Instance.LoadScene(SceneManager.GameSceneStage2);
    }
}
