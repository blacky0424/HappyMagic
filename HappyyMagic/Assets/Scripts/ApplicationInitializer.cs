using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationInitializer : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitWhile(()=> Application.isShowingSplashScreen);
        yield return new WaitForSeconds(2.0f);
        SceneManager.Instance.LoadScene(SceneManager.TitleScene);
    }
}
