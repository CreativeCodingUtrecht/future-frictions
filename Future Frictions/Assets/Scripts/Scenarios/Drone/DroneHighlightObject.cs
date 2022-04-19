using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DroneHighlightObject : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Color highlightColorTint;

    [SerializeField]
    [Range(0.1f, 5f)]
    private float highlightDuration;

    [SerializeField]
    private GameObject polaroidImage;

    private Color normalColorTint;

    private void Awake()
    {
        normalColorTint = spriteRenderer.color;
        polaroidImage.SetActive(false);
    }

    public void HandleHighlight(Queue<Action> sequence)
    {
        var seq = DOTween.Sequence().Append(spriteRenderer.DOColor(highlightColorTint, highlightDuration / 2)).Append(spriteRenderer.DOColor(normalColorTint, highlightDuration / 2)).OnComplete(() =>
        {
            polaroidImage.SetActive(true);

            var nextAction = sequence.Dequeue();
            nextAction?.Invoke();
        });
    }
}