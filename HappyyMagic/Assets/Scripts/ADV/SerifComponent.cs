using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using ADV.Command;

[System.Serializable]
public class SerifInfo
{
    public string Message;
    public string Name;
    public List<SerifComponent.CommandName> CommandNameList;
    public List<object> CommandDataList;
    
    public SerifInfo(string _name = "", string _message = "")
    {
        Message = _message;
        Name = _name;
        CommandNameList = new List<SerifComponent.CommandName>();
        CommandDataList = new List<object>();
    }

    public static SerifInfo Create(string _name = "", string _message = "")
    {
        return new SerifInfo(_name, _message);
    }
}

public class SerifComponent : MonoBehaviour
{
    public enum CommandName
    {
        BGM,
        SE,
        BGI,
        Character,
        CharacterVisible,
        Emotion,
        EmotionVisible,
        SetWindow,
        Scene,
        Close,
    }

    static readonly string[] Commands = new[]
    {
        "bgm",
        "se",
        "bgi",
        "chara",
        "charaVisible",
        "emo",
        "emoVisible",
        "setWindow",
        "scene",
        "close"
    };

    public List<SerifInfo> serifInfoList = new List<SerifInfo>();

    // 次に読み込むテキストのファイル名
    public string NextTextFileName { get; set; }

    // 誰が話しているか保持する
    public string CachedName { get; set; }

    public void Load(string filePath)
    {   // テキストを読み込んで、セリフに積む
        TextAsset textAsset = Resources.Load<TextAsset>(filePath);
        string[] senarios = textAsset.text.Split(new string[] { "@br" }, System.StringSplitOptions.None);
        for (int i = 0, max = senarios.Length; i < max; ++i)
        {
            SerifInfo serifInfo = TextAnalysis(senarios[i]);
            serifInfoList.Add(serifInfo);
        }
        Resources.UnloadAsset(textAsset);
        textAsset = null;
    }

    // 
    public void LoadRandom(string filePath)
    {   // テキストを読み込んで、セリフに積む
        TextAsset[] textAssets = Resources.LoadAll<TextAsset>(filePath);
        int index = Random.Range(0, textAssets.Length);
        Debug.Log(filePath + ":" + index);
        string[] senarios = textAssets[index].text.Split(new string[] { "@br" }, System.StringSplitOptions.None);
        for (int i = 0; i < senarios.Length; ++i)
        {
            SerifInfo serifInfo = TextAnalysis(senarios[i]);
            serifInfoList.Add(serifInfo);
        }
    }

    public string GetMessage(int index)
    {
        return serifInfoList[index].Message;
    }

    public string GetName(int index)
    {
        return serifInfoList[index].Name;
    }

    public void Clear()
    {
        if(serifInfoList == null || serifInfoList.Count == 0)
        {
            return;
        }
        serifInfoList.Clear();
    }

    public bool Exist(int currentCorsor)
    {
        return (serifInfoList.Count <= currentCorsor);
    }

    string GetCommandName(CommandName command)
    {
        return Commands[(int)command];
    }

    SerifInfo TextAnalysis(string line)
    {
        SerifInfo info = new SerifInfo();

        var lineReader = new System.IO.StringReader(line);
        var lineBuilder = new System.Text.StringBuilder();
        string text = string.Empty;
        while ((text = lineReader.ReadLine()) != null)
        {
            int commentCharacterCount = text.IndexOf("//");
            if (commentCharacterCount != -1)
            {
                text = text.Substring(0, commentCharacterCount);
            }

            if (false == string.IsNullOrEmpty(text))
            {
                // タグの行の時
                if (text[0] == '@')
                {
                    ParseCommand(info, text);
                }
                else
                {    // タグじゃない行の時
                    lineBuilder.AppendLine(text);
                }
            }
        }

        info.Name = CachedName;

        info.Message = lineBuilder.ToString();
        
        return info;
    }

    private void ParseCommand(SerifInfo info, string text)
    {
        if (text.Contains("@name"))
        {
            CachedName = ParseLine(text, "name");
        }
        else if (text.Contains("@bgm"))
        {
            info.CommandNameList.Add(CommandName.BGM);
            info.CommandDataList.Add(ParseLine(text, GetCommandName(CommandName.BGM)));
        }
        else if (text.Contains("@bgi"))
        {
            var imgComponent = transform.GetChild(0).Find("Background").GetComponent<Image>();
            var file = ParseLine(text, GetCommandName(CommandName.BGI));
            info.CommandNameList.Add(CommandName.BGI);
            info.CommandDataList.Add(new ImageChanger(imgComponent, "Image/Background/" + file));
        }
        else if (text.Contains("@charaVisible"))
        {
            var split = ParseLine(text, GetCommandName(CommandName.CharacterVisible)).Replace(" ", "").Split(',');
            var target = GetCharacterTransform(split[0]).GetComponent<Image>();
            bool enabled = bool.Parse(split[1]);

            info.CommandNameList.Add(CommandName.CharacterVisible);
            info.CommandDataList.Add(new SetVisible(target, enabled));
        }
        else if (text.Contains("@chara"))
        {
            var split = ParseLine(text, GetCommandName(CommandName.Character)).Replace(" ", "").Split(',');
            var file = "Image/Character/" + split[0];
            var target = GetCharacterTransform(split[1]).GetComponent<Image>();

            info.CommandNameList.Add(CommandName.Character);
            info.CommandDataList.Add(new ImageChanger(target, file));
        }
        else if (text.Contains("@emoVisible"))
        {
            var split = ParseLine(text, GetCommandName(CommandName.EmotionVisible)).Replace(" ", "").Split(',');
            var target = GetCharacterTransform(split[0]).GetChild(0).GetComponent<Image>();
            bool enabled = bool.Parse(split[1]);

            info.CommandNameList.Add(CommandName.EmotionVisible);
            info.CommandDataList.Add(new SetVisible(target, enabled));
        }
        else if (text.Contains("@emo"))
        {
            var split = ParseLine(text, GetCommandName(CommandName.Emotion)).Replace(" ","").Split(',');

            var file = "Image/Emotion/" + split[0];
            var target = GetCharacterTransform(split[1]).GetChild(0).GetComponent<Image>();

            info.CommandNameList.Add(CommandName.Emotion);
            info.CommandDataList.Add(new ImageChanger(target, file));
        }
        else if(text.Contains("@setWindow"))
        {
            var str = ParseLine(text, GetCommandName(CommandName.SetWindow));
            var window = transform.GetChild(0).Find("Window").gameObject;

            info.CommandNameList.Add(CommandName.SetWindow);
            info.CommandDataList.Add(new SetVisibleGameObject(window, bool.Parse(str)));
        }
        else if (text.Contains("@se"))
        {
            info.CommandNameList.Add(CommandName.SE);
            info.CommandDataList.Add(ParseLine(text, GetCommandName(CommandName.SE)));
        }
        else if (text.Contains("@scene"))
        {
            info.CommandNameList.Add(CommandName.Scene);
            info.CommandDataList.Add(ParseLine(text, GetCommandName(CommandName.Scene)));
        }
        else if (text.Contains("@close"))
        {
            info.CommandNameList.Add(CommandName.Close);
            info.CommandDataList.Add(new ADVClose(gameObject.GetComponent<ADVSystem>()));
        }
    }

    Transform GetCharacterTransform(string objName)
    {
        return transform.GetChild(0).GetChild(1).Find(objName);
    }

    public void CallCommand(int index)
    {
        if(serifInfoList[index].CommandDataList.Count != serifInfoList[index].CommandNameList.Count)
        {
            return;
        }

        for(int i = 0; i < serifInfoList[index].CommandNameList.Count; ++i)
        {
            switch(serifInfoList[index].CommandNameList[i])
            {
                case CommandName.BGM:

                    SoundManager.Instance.PlayBGM((string)serifInfoList[index].CommandDataList[i]);
                    break;
                case CommandName.SE:

                    SoundManager.Instance.PlaySE((string)serifInfoList[index].CommandDataList[i]);
                    break;
                case CommandName.BGI:
                case CommandName.Character:
                case CommandName.CharacterVisible:
                case CommandName.Emotion:
                case CommandName.EmotionVisible:
                case CommandName.SetWindow:
                case CommandName.Close:

                    ((IAdvActionable)serifInfoList[index].CommandDataList[i]).Action();
                    break;
                case CommandName.Scene:

                    SceneManager.Instance.LoadScene((string)serifInfoList[index].CommandDataList[i]);
                    break;
                default:
                    break;
            }
        }
    }

    public void OnDestroy()
    {
        // Resources.Loadで読み込んだ画像データの後処理
        foreach(var serifInfo in serifInfoList)
        {
            foreach(var commandData in serifInfo.CommandDataList)
            {
                if(commandData is IResourceUnloadable)
                {
                    ((IResourceUnloadable)commandData).Unload();
                }
            }
        }
    }

    public string ParseLine(string path = "@animation{12}", string tag = "animation")
    {
        var head = new System.Text.RegularExpressions.Regex(@tag + "{(?<numerics>.*?)}", System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);
        System.Text.RegularExpressions.Match h = head.Match(path);
        return h.Success ? h.Groups["numerics"].Value.Trim() : string.Empty;
    }
}
