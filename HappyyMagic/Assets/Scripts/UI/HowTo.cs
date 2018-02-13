using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowTo : MonoBehaviour {
    public GameObject howTo;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        howTooff();
	}

    void howTooff() {
        if (Input.GetMouseButtonUp(0)) {
            howTo.SetActive(false);
        }
    }

    public void howToOn() {
        howTo.SetActive(true);
    }
}
