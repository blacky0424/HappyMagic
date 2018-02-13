using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCountertest : MonoBehaviour {

    int m_LifeCount;
    public List<GameObject> Lifes;
    public int LifeCounttest {
        get {
            return m_LifeCount+1;
        }
    }

    // Use this for initialization
    void Start(){
        m_LifeCount = Lifes.Count - 1;
    }

	// Update is called once per frame
	void Update () {
        //テストコード、コードを整えたら削除すること。
        if (Input.GetKeyDown(KeyCode.Space)) lifeDamage();
        
	}

    public void lifeDamage() {
        if (m_LifeCount < 0){
            Debug.Log("ライフはすでにありません。");
        }
        else {
            Lifes[m_LifeCount].SetActive(false);
            m_LifeCount--;
            Debug.Log(LifeCounttest);
        }

    }

}
