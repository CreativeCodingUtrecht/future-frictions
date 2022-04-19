using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class FadePanel : MonoBehaviour
{
    public UnityEvent onFadeDone;

    public FadeType fadeType;

    public float delay = 1;
    public float fadeTime = 2;

    private Vector3 startScale;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        switch (fadeType)
        {
            case FadeType.Scale:
                startScale = transform.localScale;
                transform.localScale = Vector3.zero;
                break;
            case FadeType.Fade:
            default:
                canvasGroup = GetComponent<CanvasGroup>();
                canvasGroup.alpha = 0;
                break;
        }
    }

    private void Start()
    {
        switch (fadeType)
        {
            case FadeType.Scale:
                transform.localScale = Vector3.zero;
                break;
            case FadeType.Fade:
            default:
                canvasGroup.alpha = 0;
                break;
        }
    }

    public void FadeIn(float time)
    {
        switch (fadeType)
        {
            case FadeType.Scale:
                transform.localScale = Vector3.zero;
                transform.DOScale(startScale, time).onComplete += () => { onFadeDone?.Invoke(); };
                break;
            case FadeType.Fade:
            default:
                canvasGroup.alpha = 0;
                canvasGroup.DOFade(1, time).onComplete += () => { onFadeDone?.Invoke(); };
                break;
        }

    }

    public void Hide()
    {
        if (canvasGroup)
            canvasGroup.alpha = 0;

        if (startScale != Vector3.zero)
            transform.localScale = Vector3.zero;

        canvasGroup.DOKill();
        transform.DOKill();
    }
}

[Serializable]
public enum FadeType
{
    Fade,
    Scale
}
