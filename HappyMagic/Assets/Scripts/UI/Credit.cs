using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credit : MonoBehaviour {
    public GameObject credit;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        creditOff();
	}

    void creditOff() {
        if (Input.GetMouseButtonUp(0)) {
            credit.SetActive(false);
        }
    }

    public void creditOn() {
        credit.SetActive(true);
    }

}
