using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADVScene : MonoBehaviour
{
    [SerializeField]
    ADVSystem advSystem;

    void Start()
    {
        advSystem.ADVOpen();
        FadeManager.Instance.FadeIn();
    }
}
