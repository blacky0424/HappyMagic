using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour {

	enum TriggerType{
		Trigger1,
		Trigger2,
		Trigger3,
		Trigger4,
		Trigger5,
		Trigger6,
		End,
	}
	[SerializeField]
	TriggerType type;
    [SerializeField]
    TriggerType currentType;

    void Start()
    {
        Restart();
    }

    public void Restart()
    {
        currentType = type;
    }

	void OnTriggerEnter(Collider c) {
        if (c.tag == TagName.Player || c.tag == TagName.Fake)
        {
            switch (currentType)
            {
                case TriggerType.Trigger1:
                    TutorialScene.Instance.Tutorial2();
                    break;
                case TriggerType.Trigger2:
                    TutorialScene.Instance.Tutorial3();
                    break;
                case TriggerType.Trigger3:
                    TutorialScene.Instance.Tutorial4();
                    break;
                case TriggerType.Trigger4:
                    TutorialScene.Instance.Tutorial5();
                    break;
                case TriggerType.Trigger5:
                    TutorialScene.Instance.Tutorial6();
                    break;
                case TriggerType.Trigger6:
                    TutorialScene.Instance.Tutorial7();
                    break;
            }

            currentType = TriggerType.End;
        }
	}
}
