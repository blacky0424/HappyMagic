using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeManager : SingletonMonoBehaviour<FadeManager>
{
    public float m_fadeTime = 0.5f;
    public Color m_fadeColor = Color.black;
    [SerializeField]
    Image m_fadeImage;
    CanvasGroup m_canvas;

    public bool IsFading { get; private set; }

    void Start()
    {
        m_canvas = GetComponent<CanvasGroup>();
        m_fadeImage.color = m_fadeColor;
        m_canvas.alpha = 1;

        FadeIn();
    }

    public void ChangeFadeColor(Color c)
    {
        m_fadeImage.color = c;
    }

    public void FadeIn(System.Action callback = null)
    {
        IsFading = true;
        // 1 to 0
        m_canvas.DOFade(0.0f, m_fadeTime).SetUpdate(true).OnComplete(() =>
        {
            if (callback != null)
            {
                callback();
            }

            IsFading = false;
        });
    }

    public void FadeOut(System.Action callback = null)
    {
        IsFading = true;
        // 0 to 1
        m_canvas.DOFade(1.0f, m_fadeTime).SetUpdate(true).OnComplete(() =>
        {
            if (callback != null)
            {
                callback();
            }
            IsFading = false;
        });
    }

#if UNITY_EDITOR
    private void Reset()
    {
        m_fadeImage = transform.GetComponentInChildren<Image>();
    }
#endif
}