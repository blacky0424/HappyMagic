using UnityEngine;

[DefaultExecutionOrder(-101)]
[RequireComponent(typeof(SerifComponent))]
public class TextReader : MonoBehaviour
{
    SerifComponent serifComponent;

    private void Start()
    {
        serifComponent = GetComponent<SerifComponent>();
    }

    public void Load(string filePath = "chapter_0_0")
    {
        serifComponent.Clear();
        serifComponent.Load(filePath);
    }

    public void ReadText(int index, ref string nameText, ref string serifText)
    {
        if(serifComponent.Exist(index))
        {
            return;
        }

        nameText = serifComponent.serifInfoList[index].Name;
        serifText = serifComponent.serifInfoList[index].Message;

        serifComponent.CallCommand(index);
    }
}
