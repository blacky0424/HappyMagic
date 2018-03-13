using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : SingletonMonoBehaviour<SceneManager>
{
    public static new SceneManager Instance
    {
        get
        {
            if(instance != null) { return instance; }
            instance = FindObjectOfType<SceneManager>();
            if(instance == null)
            {
                var obj = new GameObject(typeof(SceneManager).Name);
                instance = obj.AddComponent<SceneManager>();
            }
            return instance;
        }
    }
    
    public static AsyncOperation LoadSceneAsync(string sceneName)
    {
        return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
    }

    protected new void Awake()
    {
        if(CheckInstance())
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public const string TitleScene         = "TitleScene";
    public const string ADVScene           = "ADVScene";
    public const string ADV2Scene           = "ADV2Scene";
    public const string StageSelectScene   = "StageSelectScene";
    public const string TutorialScene      = "Tutorial";
    public const string GameSceneStage1    = "Stage1";
    public const string GameSceneStage2    = "Stage2";
    public const string ClearMovieScene    = "ClearMovieScene";
    public const string ClearScene         = "ClearScene";
    public const string ResultScene        = "ResultScene";
    public const string TrueEndScene       = "TrueEndScene";
    public const string PrinceEndScene     = "PrinceEndScene";
    public const string TutorialClearScene = "TutorialClearScene";
    public const string GameOverScene = "GameOverScene";
    public const string ConnectScene = "ConnectScene";

    bool m_isLoading;

    /// <summary>
    /// シーンのロード。呼び出すべきシーンはSceneManagerの定数を参照。
    /// 例：SceneManager.Instance.LoadScene(SceneManager.TITLE_SCENE);
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName)
    {
        if (FadeManager.Instance == null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
        else
        {
            if (m_isLoading) { return; }
            m_isLoading = true;
            FadeManager.Instance.FadeOut(() => 
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
                m_isLoading = false;
            });
        }
    }
}
