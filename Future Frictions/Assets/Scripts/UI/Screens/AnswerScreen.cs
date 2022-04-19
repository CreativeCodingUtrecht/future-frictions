using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerScreen : BaseScreen
{
    public Button buttonA;
    public Button buttonB;
    public Button buttonC;

    [SerializeField]
    private TMP_Text optionA;

    [SerializeField]
    private TMP_Text optionB;

    [SerializeField]
    private TMP_Text optionC;

    public void Init(string a, string b, string c)
    {
        optionA.text = a;
        optionB.text = b;
        optionC.text = c;

        buttonA.onClick.RemoveAllListeners();
        buttonB.onClick.RemoveAllListeners();
        buttonC.onClick.RemoveAllListeners();
    }
}