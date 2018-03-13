using UnityEngine;
using UnityEngine.UI;
using System.Text;

[DefaultExecutionOrder(-100)]
public class ADVSystem : MonoBehaviour {

    [SerializeField]
    float viewMessageTime = 0.1f;

    [SerializeField]
    string serifFilePath = "Text/chapter_0_0";
    [SerializeField]
    string skipMsgFilePath = "Text/chapter_0_1";

    [SerializeField]
    Text nameField;
    [SerializeField]
    Text msgField;
    [SerializeField]
    Text skipMessage;
    [SerializeField]
    GameObject skipMenu;

    int textIndex;
    bool isSkipEnabled = false;

    int showMsgIndex;
    float msgViewElapsedTime;

    TextReader reader;
    string nameText;
    string msgText;
    StringBuilder msgBuilder = new StringBuilder();


    bool HasSkipMenuOpen
    {
        get
        {
            return skipMenu.activeSelf;
        }
    }

    void Start ()
    {
        reader = GetComponent<TextReader>();
        Init();
        ADVClose();
	}

    void Init()
    {
        textIndex = 0;
        msgViewElapsedTime = 0f;

        if(reader == null)
        {
            reader = GetComponent<TextReader>();
        }
        reader.Load(serifFilePath);
        UpdateText();

        LoadSkipMsgText();

        skipMenu.SetActive(false);
    }

    void LoadSkipMsgText()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(skipMsgFilePath);
        skipMessage.text = textAsset.text;
        Resources.UnloadAsset(textAsset);
        textAsset = null;
    }
	
	void Update ()
    {
        if(HasSkipMenuOpen)
        {
            return;
        }

        if (isSkipEnabled)
        {
            UpdateMessage();
            msgField.text = msgBuilder.ToString();
            return;
        }

        if(!IsShowingAllMsg() && IsShowNextChar())
        {
            msgViewElapsedTime = 0f;
            msgBuilder.Append(msgText[showMsgIndex]);
            showMsgIndex++;
        }

        msgViewElapsedTime += Time.deltaTime;

        msgField.text = msgBuilder.ToString();
    }

    void UpdateMessage()
    {
        if (IsShowingAllMsg())
        {
            showMsgIndex = 0;
            msgBuilder.Length = 0;

            textIndex++;
            UpdateText();
        }
        else
        {
            msgBuilder.Length = 0;
            msgBuilder.Append(msgText);
        }
    }

    void UpdateText()
    {
        reader.ReadText(textIndex, ref nameText, ref msgText);
        nameField.text = nameText;
    }

    bool IsShowingAllMsg()
    {
        return msgBuilder.Length == msgText.Length;
    }

    bool IsShowNextChar()
    {
        return viewMessageTime <= msgViewElapsedTime;
    }

    public void ADVOpen()
    {
        gameObject.SetActive(true);
        Init();
    }

    public void ADVOpen(string serifFilePath, string skipMsgFilePath)
    {
        this.serifFilePath = serifFilePath;
        this.skipMsgFilePath = skipMsgFilePath;
        ADVOpen();
    }

    public void ADVClose()
    {
        gameObject.SetActive(false);
    }

    public void OnViewNextMessage()
    {
        SoundManager.Instance.PlaySE(SEName.Tap);
        UpdateMessage();
    }

    public void OnSkipButton()
    {
        skipMenu.SetActive(true);
    }

    public void OnSkipEnabled()
    {
        isSkipEnabled = true;
        OnSkipCanceled();
    }

    public void OnSkipCanceled()
    {
        skipMenu.SetActive(false);
    }
}
