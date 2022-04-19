using System;
using UnityEngine;

public class Square : MonoBehaviour
{
    [SerializeField]
    private SquareObject[] squareObjects;

    public void SetSquareState(Scenarios scenario, Action callback = null)
    {
        foreach (var sO in squareObjects)
        {
            if (sO.scenario == scenario)
            {
                sO.sequence.StartSequence(callback);
            }
            else
            {
                sO.sequence.Hide();
            }
        }
    }
}

[Serializable]
public class SquareObject
{
    public Scenarios scenario;
    public FadeSequence sequence;
}
