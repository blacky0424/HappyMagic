using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    bool goal = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!goal)
        {
            //チュートリアルか確認
            TutorialScene t = FindObjectOfType<TutorialScene>();
            if (other.gameObject.tag == TagName.Player || other.gameObject.tag == TagName.Fake)
            {
                //何度も呼ばれないように　
                goal = true;
                ScoreManager.Instance.SetTimeScore();
                ScoreManager.Instance.SetDiscoveryScore();

                if (t == null)
                {
                    SoundManager.Instance.StopBGM();
                    SceneManager.Instance.LoadScene(SceneManager.ClearMovieScene);
                }
                else
                {
                    SceneManager.Instance.LoadScene(SceneManager.TutorialClearScene);
                }

            }
        }
    }
}
