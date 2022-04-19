using UnityEngine;
using DG.Tweening;
using System;

public class PulseScale : MonoBehaviour
{
    [SerializeField]
    private Vector3 targetScale;

    [SerializeField]
    [Range(.5f, 10f)]
    private float animationDuration;

    [SerializeField]
    private Ease easeMode;

    private Vector3 startScale;
    private Sequence sequence;

    private void Awake()
    {
        startScale = transform.localScale;
    }

    private void Start()
    {
        RunAnimation();
    }

    public void RunAnimation()
    {
        sequence = DOTween.Sequence().Append(transform.DOScale(targetScale.y, animationDuration).SetEase(easeMode)).Append(transform.DOScale(startScale.y, animationDuration).SetEase(easeMode)).SetLoops(-1);
    }

    public void SetSelectedState(bool selected, Action backInPostionAction)
    {
        if (selected)
        {
            sequence.Pause();
            transform.DOScale(startScale.y, animationDuration / 2).OnComplete(() => { backInPostionAction?.Invoke(); });
        }
        else
        {
            sequence.Play();
        }
    }
}