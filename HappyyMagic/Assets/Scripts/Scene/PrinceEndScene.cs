﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinceEndScene : MonoBehaviour
{

    [SerializeField]
    ADVSystem advSys;

	void Start ()
    {
        advSys.ADVOpen();
        FadeManager.Instance.FadeIn();
	}
}