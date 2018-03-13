using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {

    [SerializeField]
    ParticleSystem particle;

    void OnEnable()
    {
        gameObject.transform.rotation = Quaternion.Euler(-90.0f, 0, 0);
        StartCoroutine(StartParticle());
    }

    IEnumerator StartParticle()
    {
        yield return new WaitWhile(() => particle.IsAlive(true));
        gameObject.SetActive(false);
    }
}
