using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinderellaPumpkinViewer : MonoBehaviour {

    [SerializeField]
    GameObject pumpkin;
    [SerializeField]
    AnimationCurve animationCurve = AnimationCurve.Linear(0, 0, 0.5f, 1);

    Material pumpkinMaterial;
    Color color;

    IEnumerator fadeEnumerator;

    private void Start()
    {
        pumpkinMaterial = pumpkin.GetComponent<MeshRenderer>().material;
        color = pumpkinMaterial.color;
        color.a = 0f;

        OnHidePumpkinObj();
    }

    private void Update()
    {
        if(!pumpkin.activeSelf)
        {
            return;
        }

        float normalizedTime = GameScene.Instance.Cinderella.GetAnimationTime;

        color.a = animationCurve.Evaluate(normalizedTime);
        pumpkinMaterial.color = color;
    }

    private void OnDisable()
    {
        OnHidePumpkinObj();
    }

    public void OnShowPumpkinObj()
    {
        pumpkin.SetActive(true);
        color.a = 0.0f;
        pumpkinMaterial.color = color;
    }

    public void OnHidePumpkinObj()
    {
        pumpkin.SetActive(false);
    }
}
