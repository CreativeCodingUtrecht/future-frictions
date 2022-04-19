using DG.Tweening;
using UnityEngine;

public class AnimateUpAndDown : MonoBehaviour
{
    [SerializeField]
    private Vector3 targetPosition;

    [SerializeField]
    [Range(.5f, 10f)]
    private float animationDuration;

    [SerializeField]
    private Ease easeMode;

    private float startY;
    private Sequence sequence;

    public void RunAnimation()
    {
        sequence = DOTween.Sequence().Append(transform.DOLocalMoveY(targetPosition.y, animationDuration).SetEase(easeMode)).Append(transform.DOLocalMoveY(transform.position.y, animationDuration).SetEase(easeMode)).SetLoops(-1);
    }

    public void StopAnimation()
    {
        sequence.Pause();
    }
}