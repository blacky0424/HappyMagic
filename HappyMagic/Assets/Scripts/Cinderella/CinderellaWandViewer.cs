using UnityEngine;

public class CinderellaWandViewer : MonoBehaviour {

    [SerializeField]
    GameObject wand;

    public void OnShowWandObj()
    {
        wand.SetActive(true);
    }

    public void OnHideWandObj()
    {
        wand.SetActive(false);
    }
}
