using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionScreen : BaseScreen
{
    [SerializeField] private GameObject[] polaroids;

    [SerializeField] private Image[] avatarImages;

    [Header("Elements")] [SerializeField] private TextMeshProUGUI questionText;

    [SerializeField] private TextMeshProUGUI answerA;

    [SerializeField] private TextMeshProUGUI answerB;

    [SerializeField] private TextMeshProUGUI answerC;

    [SerializeField] private Toggle optionAToggle;

    [SerializeField] private Image optionAImage;

    [SerializeField] private Toggle optionBToggle;

    [SerializeField] private Image optionBImage;

    [SerializeField] private Toggle optionCToggle;

    [SerializeField] private Image optionCImage;

    [SerializeField] private Button submitButton;

    [SerializeField] private Drone drone;

    [SerializeField] private Cat cat;

    [SerializeField] private Pigeon pigeon;

    [SerializeField] private Sprite neutralSprite;

    [SerializeField] private Sprite selectedSprite;

    [SerializeField] private NextButton nextButton;

    private ScenarioOptions selectedOption = ScenarioOptions.NONE;

    private void Awake()
    {
        optionAToggle.onValueChanged.AddListener(state =>
        {
            if (state)
            {
                selectedOption = ScenarioOptions.OptionA;
                optionAImage.sprite = selectedSprite;

                optionBToggle.SetIsOnWithoutNotify(false);
                optionBImage.sprite = neutralSprite;
                optionCToggle.SetIsOnWithoutNotify(false);
                optionCImage.sprite = neutralSprite;
            }
            else
            {
                selectedOption = ScenarioOptions.NONE;
                optionAImage.sprite = neutralSprite;
            }

            UpdateSubmitButton();
            // Debug.Log($"Option: {selectedOption}");
        });

        optionBToggle.onValueChanged.AddListener(state =>
        {
            if (state)
            {
                selectedOption = ScenarioOptions.OptionB;
                optionBImage.sprite = selectedSprite;

                optionAToggle.SetIsOnWithoutNotify(false);
                optionAImage.sprite = neutralSprite;
                optionCToggle.SetIsOnWithoutNotify(false);
                optionCImage.sprite = neutralSprite;
            }
            else
            {
                selectedOption = ScenarioOptions.NONE;
                optionBImage.sprite = neutralSprite;
            }

            UpdateSubmitButton();
            // Debug.Log($"Option: {selectedOption}");
        });

        optionCToggle.onValueChanged.AddListener(state =>
        {
            if (state)
            {
                selectedOption = ScenarioOptions.OptionC;
                optionCImage.sprite = selectedSprite;

                optionAToggle.SetIsOnWithoutNotify(false);
                optionAImage.sprite = neutralSprite;
                optionBToggle.SetIsOnWithoutNotify(false);
                optionBImage.sprite = neutralSprite;
            }
            else
            {
                selectedOption = ScenarioOptions.NONE;
                optionCImage.sprite = neutralSprite;
            }

            UpdateSubmitButton();
            // Debug.Log($"Option: {selectedOption}");
        });

        submitButton.onClick.AddListener(() =>
        {
            switch (EnvironmentChanges.Instance.ActiveScenario.ScenarioID)
            {
                // Check which scenario is active
                case Scenarios.Drone:
                {
                    if (drone) drone.SetSelectedOption(selectedOption);
                    break;
                }
                case Scenarios.Cat:
                {
                    if (cat) cat.SetSelectedOption(selectedOption);
                    break;
                }
                case Scenarios.Pigeon:
                {
                    if (pigeon) pigeon.SetSelectedOption(selectedOption);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        });
        
        nextButton.SetColor(submitButton.interactable);
    }

    public void SetTextData(string scenarioID, Sprite[] avatarImageSprites)
    {
        var scenarioData = ApplicationManager.scenariosTextData[scenarioID];

        UIManager.Instance.CloseDialog();

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

        for (var i = 0; i < avatarImages.Length; i++) avatarImages[i].sprite = avatarImageSprites[i];
    }

    private void UpdateSubmitButton()
    {
        submitButton.interactable = selectedOption != ScenarioOptions.NONE;
        nextButton.SetColor(submitButton.interactable);
    }

    public override void Close()
    {
        base.Close();

        foreach (var polaroid in polaroids) polaroid.SetActive(false);
    }
}