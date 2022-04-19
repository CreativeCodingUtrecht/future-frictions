using System;
using DG.Tweening;
using UnityEngine;

public class CatAnimation : MonoBehaviour
{
  [SerializeField]
  private Vector3 targetPosition;

  [SerializeField]
  [Range(.5f, 10f)]
  private float animationDuration;

  [SerializeField]
  private Ease easeMode;

  private Vector3 startPosition;
  private Sequence sequence;

  private void Awake()
  {
    startPosition = transform.position;
  }

  private void Start()
  {
    RunAnimation();
  }

  public void RunAnimation()
  {
    sequence = DOTween.Sequence().Append(transform.DOLocalMoveY(targetPosition.y, animationDuration).SetEase(easeMode)).Append(transform.DOLocalMoveY(startPosition.y, animationDuration).SetEase(easeMode)).SetLoops(-1);
  }

  public void SetSelectedState(bool selected, Action backInPostionAction)
  {
    if (selected)
    {
      sequence.Pause();
      transform.DOLocalMoveY(startPosition.y, animationDuration / 2).OnComplete(() => { backInPostionAction?.Invoke(); });
            
    }
    else
    {
      sequence.Play();
    }
  }
}