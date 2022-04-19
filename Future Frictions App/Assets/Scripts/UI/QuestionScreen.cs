using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionScreen : BaseScreen
{
    [SerializeField]
    private Image[] avatarImages;

    [Header("Elements")]
    [SerializeField]
    private TextMeshProUGUI questionText;

    [SerializeField]
    private TextMeshProUGUI answerA;

    [SerializeField]
    private TextMeshProUGUI answerB;

    [SerializeField]
    private TextMeshProUGUI answerC;

    [SerializeField]
    private Toggle optionAToggle;

    [SerializeField]
    private Image optionAImage;

    [SerializeField]
    private Toggle optionBToggle;

    [SerializeField]
    private Image optionBImage;

    [SerializeField]
    private Toggle optionCToggle;

    [SerializeField]
    private Image optionCImage;

    [SerializeField]
    private Button submitButton;

    [SerializeField]
    private Sprite neutralSprite;

    [SerializeField]
    private Sprite selectedSprite;

    [SerializeField]
    private Square square;

    [Header("Sequence Items")]
    [SerializeField]
    private FadePanel[] fadePanels;

    [SerializeField]
    private NextButton nextButton;
    
    private Options selectedOption = Options.NONE;

    private ScenarioData currentData;

    public void InitializeQuestion()
    {
        currentData = AppManager.Instance.dataManager.GetQuestionData();

        if (currentData != null)
        {
            AnimateIn();

            SetTextData(currentData.Scenario.ToString(), currentData.avatarImages);
        }
        else
        {
            AppManager.Instance.uiManager.SetScreen(Screens.BULLETIN);
        }
    }

    public void SetTextData(string scenarioID, Sprite[] avatarImageSprites)
    {
        var scenarioData = DataManager.scenariosTextData[scenarioID];

        // Set the question
        questionText.text = scenarioData.beforeText;

        // Set the answers texts
        answerA.text = scenarioData.optionA;
        answerB.text = scenarioData.optionB;
        answerC.text = scenarioData.optionC;

        submitButton.interactable = false;

        optionAToggle.isOn = false;
        optionAImage.sprite = neutralSprite;
        optionBToggle.isOn = false;
        optionBImage.sprite = neutralSprite;
        optionCToggle.isOn = false;
        optionCImage.sprite = neutralSprite;

        optionAToggle.onValueChanged.AddListener((state) =>
        {
            if (state)
            {
                selectedOption = Options.A;
                optionAImage.sprite = selectedSprite;

                optionBToggle.SetIsOnWithoutNotify(false);
                optionBImage.sprite = neutralSprite;
                optionCToggle.SetIsOnWithoutNotify(false);
                optionCImage.sprite = neutralSprite;
            }
            else
            {
                selectedOption = Options.NONE;
                optionAImage.sprite = neutralSprite;
            }

            UpdateSubmitButton();
        });

        optionBToggle.onValueChanged.AddListener((state) =>
        {
            if (state)
            {
                selectedOption = Options.B;
                optionBImage.sprite = selectedSprite;

                optionAToggle.SetIsOnWithoutNotify(false);
                optionAImage.sprite = neutralSprite;
                optionCToggle.SetIsOnWithoutNotify(false);
                optionCImage.sprite = neutralSprite;
            }
            else
            {
                selectedOption = Options.NONE;
                optionBImage.sprite = neutralSprite;
            }

            UpdateSubmitButton();
        });

        optionCToggle.onValueChanged.AddListener((state) =>
        {
            if (state)
            {
                selectedOption = Options.C;
                optionCImage.sprite = selectedSprite;

                optionAToggle.SetIsOnWithoutNotify(false);
                optionAImage.sprite = neutralSprite;
                optionBToggle.SetIsOnWithoutNotify(false);
                optionBImage.sprite = neutralSprite;
            }
            else
            {
                selectedOption = Options.NONE;
                optionCImage.sprite = neutralSprite;
            }

            UpdateSubmitButton();
        });

        submitButton.onClick.AddListener(() =>
        {
            currentData.SelectedOption = selectedOption;

            AppManager.Instance.uiManager.SetScreen(Screens.RESULT);
        });
        
        nextButton.SetColor(submitButton.interactable);

        if (avatarImageSprites != null && avatarImageSprites.Length == avatarImages.Length)
        {
            for (int i = 0; i < avatarImages.Length; i++)
            {
                avatarImages[i].sprite = avatarImageSprites[i];
            }
        }
    }

    public void AnimateIn()
    {
        StartCoroutine(WaitAndHandleNext(0, () =>
        {
            square.SetSquareState(currentData.Scenario, () =>
            {
                var sequence = new Queue<Action>();

                for (int i = 0; i < fadePanels.Length; i++)
                {
                    int local = i;

                    sequence.Enqueue(() =>
                    {
                        StartCoroutine(WaitAndHandleNext(fadePanels[local].delay, () =>
                        {
                            fadePanels[local].FadeIn(fadePanels[local].fadeTime);

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
            });
        }));
    }

    private IEnumerator WaitAndHandleNext(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    public override void Close()
    {
        base.Close();

        StopAllCoroutines();

        square.SetSquareState(Scenarios.NONE);

        foreach (var fP in fadePanels)
        {
            fP.Hide();
        }
    }

    public void UpdateSubmitButton()
    {
        submitButton.interactable = selectedOption != Options.NONE;
        nextButton.SetColor(submitButton.interactable);
    }
}
