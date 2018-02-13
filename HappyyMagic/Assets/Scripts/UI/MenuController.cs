using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    [SerializeField]
    GameObject menuObject;

    public bool IsMenuOpen { get; private set; }

    private void Start()
    {
        menuObject.SetActive(false);
    }

    private void Update()
    {
        if(FadeManager.Instance.IsFading && IsMenuOpen)
        {
            MenuClose();
        }
    }

    public void MenuOpen()
    {
        if (FadeManager.Instance.IsFading) { return; }
        IsMenuOpen = true;
        menuObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void MenuClose()
    {
        IsMenuOpen = false;
        menuObject.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void ReturnTitle()
    {
        Time.timeScale = 1.0f;
        SceneManager.Instance.LoadScene(SceneManager.TitleScene);
    }

    public void SkipTutorial()
    {
        FadeManager.Instance.FadeOut();
        SceneManager.Instance.LoadScene(SceneManager.ADV2Scene);
    }
}
