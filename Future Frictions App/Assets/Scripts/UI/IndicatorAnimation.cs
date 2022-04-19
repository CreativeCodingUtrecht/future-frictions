using UnityEngine;
using DG.Tweening;

public class IndicatorAnimation : MonoBehaviour
{
    [SerializeField]
    private float scaleDuration = 2f;

    [SerializeField]
    private float moveDuration = 2f;

    [SerializeField]
    private float zeroPosY = 0;

    [SerializeField]
    private float zeroScale = 0;

    private float normalPosY;
    private float startScale;

    private void Awake()
    {
        var trans = transform;
        normalPosY = trans.localPosition.y;
        startScale = trans.localScale.y;

        gameObject.SetActive(false);
    }

    public void AnimateIn()
    {
        // Scale up and move up
        var trans = (RectTransform)transform;

        trans.localPosition = new Vector2(trans.localPosition.x, zeroPosY);
        trans.localScale = Vector2.zero * zeroScale;

        gameObject.SetActive(true);

        trans.DOScale(startScale, scaleDuration);
        trans.DOLocalMoveY(normalPosY, moveDuration, true);
    }

    public void AnimateOut()
    {
        // Scale down and move down
        var trans = transform as RectTransform;

        trans.DOScale(zeroScale, scaleDuration);
        trans.DOLocalMoveY(zeroPosY, moveDuration, true);
    }
}
