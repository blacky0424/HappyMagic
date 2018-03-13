using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPrint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == TagName.Player || other.gameObject.tag == TagName.Fake)
        {
            ScoreManager.Instance.FootPrint++;
            this.gameObject.SetActive(false);
        }
    }
}
