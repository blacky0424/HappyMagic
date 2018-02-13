using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGMName
{
    ADVCastle,
    ADVNight,
    Treasure,
    Title,
    Gameplay,
    Result,
}

public enum SEName
{
    Tap,
    GameClear,
    Discover,
    Destruction123,
    Destruction45,
    DestrFx,
    Change,
    Mouse,
    Smoke,
    Landing,
    Bell,
}

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    #region Singleton
    public static new SoundManager Instance
    {
        get
        {
            if(instance != null) { return instance; }
            instance = FindObjectOfType<SoundManager>();
            if(instance == null)
            {
                Debug.LogError("SoundManagerがありません。適切な形でシーンに配置してください。");
            }
            return instance;
        }
    }

    protected new void Awake()
    {
        if(CheckInstance())
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion//Singleton

    static readonly Dictionary<BGMName, string> bgmNameDict = new Dictionary<BGMName, string>
    {
        {BGMName.ADVCastle, "01_ADV_Castle" },
        {BGMName.ADVNight,  "02_ADV_Night"  },
        {BGMName.Treasure,  "03_Treasure"   },
        {BGMName.Title,     "04_Title"      },
        {BGMName.Gameplay,  "05_Gameplay"   },
        {BGMName.Result,    "06_Result"     },
    };

    static readonly List<string> seNameDict = new List<string>
    {
        "01_Tap",
        "02_GameClear",
        "03_Discovery",
        "04_Destruction_1-2-3",
        "05_Destruction_4-5",
        "06_DestrFx",
        "07_Change",
        "08_Mouse",
        "09_Smoke",
        "10_Landing",
        "11_Bell",
    };

    [SerializeField]
    CriAtomSource m_bgmSource;
    [SerializeField]
    CriAtomSource m_seSource;

    public void PlayBGM(string cueName)
    {
        m_bgmSource.Play(cueName);
    }

    public void PlayBGM(BGMName cueName)
    {
        PlayBGM(bgmNameDict[cueName]);
    }

    public void StopBGM()
    {
        m_bgmSource.Stop();
    }

    public void StopSE()
    {
        m_seSource.Stop();
    }

    public void PlaySE(string cueName)
    {
        m_seSource.Play(cueName);
    }

    public void PlaySE(SEName cueName)
    {
        PlaySE(seNameDict[(int)cueName]);
    }
}
