using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title_controll : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        StartGame();
	}

    void StartGame() {
        if (!Input.GetMouseButtonDown(0)) return;
        SceneManager.Instance.LoadScene("StageSelectScene");
    }
}
