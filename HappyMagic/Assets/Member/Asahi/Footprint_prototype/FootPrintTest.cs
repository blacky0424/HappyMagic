using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPrintTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision Other){
        
        if (Other.gameObject.tag == "Player") {
            this.gameObject.SetActive(false);
            ScoreManager.Instance.FootPrint++;
        }

    }
}
