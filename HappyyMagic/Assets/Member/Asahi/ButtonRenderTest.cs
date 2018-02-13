using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonRenderTest : MonoBehaviour
{

    public Button m_hide;
    public Button m_feint;
    public Button m_camouflage;
    public Button m_listen;


    public List<GameObject> Icon;
    float Transparency = 1.0f;

    List<Button> buttonBox = new List<Button>();



    void Start()
    {
        buttonBox.Add(m_hide);
        buttonBox.Add(m_feint);
        buttonBox.Add(m_camouflage);
        buttonBox.Add(m_listen);

    }

     void Update(){

        if (Input.GetKeyDown("space")) {
            ButtonNotActive();
        }
        if (Input.GetKeyDown("a")) {
            ButtonActive();
        }

      }


    /// <summary>
    /// コマンドボタンが押せない状態へ
    /// </summary>
    void ButtonNotActive()
    {
        Transparency = 0.5f;
        for(int i = 0;i < buttonBox.Count; i++)
        {
            buttonBox[i].interactable = false;
            Icon[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, Transparency);
        }
    }

    /// <summary>
    /// コマンドボタンが押せる状態へ
    /// </summary>
    public void ButtonActive()
    {
        Transparency = 1.0f;
        for (int i = 0; i < buttonBox.Count; i++)
        {
            buttonBox[i].interactable = true;
            Icon[i].GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, Transparency);
        }
    }
}
