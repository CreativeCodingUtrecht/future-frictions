using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FadeSequence : MonoBehaviour
{
    [SerializeField]
    private float doneDelay = 5f;

    [SerializeField]
    private FadePanel[] fadePanels;

    [SerializeField]
    private FadePanel technology;

    private List<InteractableCharacters> talkedTo = new List<InteractableCharacters>();
    private Action sequenceCallback;

    public void StartSequence(Action callback)
    {
        sequenceCallback = callback;

        gameObject.SetActive(true);

        Queue<Action> sequence = new Queue<Action>();

        for (int i = 0; i < fadePanels.Length; i++)
        {
            int local = i;

            sequence.Enqueue(() =>
            {
                StartCoroutine(WaitAndHandleNext(fadePanels[local].delay, () =>
                {
                    fadePanels[local].FadeIn(2);

                    if (sequence.Count > 0)
                    {
                        var nextAction = sequence.Dequeue();
                        nextAction?.Invoke();
                    }
                }));
            });
        }

        if (sequence.Count > 0)
        {
            sequence.Reverse();

            var nextAction = sequence.Dequeue();
            nextAction?.Invoke();
        }
    }

    public void UpdateTalkedCount(InteractableCharacters character)
    {
        if (!talkedTo.Contains(character))
        {
            talkedTo.Add(character);
        }

        if (talkedTo.Count >= 3)
        {
            StartCoroutine(WaitAndHandleNext(doneDelay, () =>
            {
                technology.FadeIn(2);
            }));
        }
    }

    public void ShowDialogue()
    {
        sequenceCallback?.Invoke();
    }

    public void Hide()
    {
        StopAllCoroutines();

        gameObject.SetActive(false);

        foreach (var fP in fadePanels)
        {
            fP.Hide();
        }
    }

    private IEnumerator WaitAndHandleNext(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
}
