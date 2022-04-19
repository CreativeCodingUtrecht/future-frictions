using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class IntroScreen : BaseScreen
{
    [SerializeField]
    private GameObject introFormPanel;

    [SerializeField]
    private GameObject introTextPanel;

    [SerializeField]
    private Button startButton;

    [SerializeField]
    private TMP_InputField[] inputFields;

    [SerializeField]
    private GameObject introGroup;

    [SerializeField]
    private GameObject loading;

    [SerializeField]
    private StartScreenTextElement[] startScreenTextElements;

    private List<TMP_InputField> filledFields = new List<TMP_InputField>();

    private void Awake()
    {
        startButton.interactable = false;

        introGroup.SetActive(false);
        loading.SetActive(true);

        foreach (var input in inputFields)
        {
            input.onEndEdit.AddListener((value) => { CheckFormComplete(input, value); });
        }

        startButton.onClick.AddListener(() =>
        {
            AppManager.Instance.uiManager.SetScreen(Screens.QUESTION);
            var questionScreen = AppManager.Instance.uiManager.GetScreen<QuestionScreen>();
            questionScreen.InitializeQuestion();

            DataManager.SetUserData(new UserData()
            {
                Name = inputFields[0].text,
                Age = inputFields[1].text,
                Email = inputFields[2].text,
                Location = inputFields[3].text
            });
        });

        introFormPanel.SetActive(true);
        introTextPanel.SetActive(false);

        DataManager.AppTextAvailable += OnAppTextDataAvailable;
        DataManager.LanguageChanged += OnLanguageChanged;
    }

    private void OnLanguageChanged()
    {
        OnAppTextDataAvailable();
    }

    private void OnAppTextDataAvailable()
    {
        foreach (var textElement in startScreenTextElements)
        {
            textElement.label.text = DataManager.otherTextData[textElement.id];
        }

        introGroup.SetActive(true);
        loading.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            EventSystem system = EventSystem.current;
            GameObject curObj = system.currentSelectedGameObject;
            GameObject nextObj = null;
            if (!curObj)
            {
                nextObj = system.firstSelectedGameObject;
            }
            else
            {
                Selectable curSelect = curObj.GetComponent<Selectable>();
                Selectable nextSelect =
                    Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)
                        ? curSelect.FindSelectableOnUp()
                        : curSelect.FindSelectableOnDown();
                if (nextSelect)
                {
                    nextObj = nextSelect.gameObject;
                }
            }
            if (nextObj)
            {
                system.SetSelectedGameObject(nextObj, new BaseEventData(system));
            }
        }
    }

    private void CheckFormComplete(TMP_InputField field, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            if (filledFields.Contains(field))
            {
                filledFields.Remove(field);
            }

            startButton.interactable = false;

            return;
        }

        if (!filledFields.Contains(field))
        {
            filledFields.Add(field);
        }

        startButton.interactable = filledFields.Count == inputFields.Length;
    }

    private void OnDestroy()
    {
        DataManager.AppTextAvailable -= OnAppTextDataAvailable;
        DataManager.LanguageChanged -= OnLanguageChanged;
    }
}

[Serializable]
public struct StartScreenTextElement
{
    public string id;
    public TextMeshProUGUI label;
}