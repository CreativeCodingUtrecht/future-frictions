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
    private Button introTextButton;

    [SerializeField]
    private TMP_InputField[] inputFields;

    [SerializeField]
    private GameObject introGroup;

    [SerializeField]
    private GameObject loading;

    [SerializeField]
    private StartScreenTextElement[] startScreenTextElements;

    private UIManager uiManager;
    private QuestManager questManager;

    private List<TMP_InputField> filledFields = new List<TMP_InputField>();

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        questManager = FindObjectOfType<QuestManager>();

        startButton.interactable = false;

        introGroup.SetActive(false);
        loading.SetActive(true);

        foreach (var input in inputFields)
        {
            input.onEndEdit.AddListener((value) => { CheckFormComplete(input, value); });
        }

        startButton.onClick.AddListener(() =>
        {
            var userData = ApplicationManager.Instance.userData;
            userData.Name = inputFields[0].text;
            userData.Age = inputFields[1].text;
            userData.Email = inputFields[2].text;
            userData.Location = inputFields[3].text;

            introFormPanel.SetActive(false);
            introTextPanel.SetActive(true);
        });

        introTextButton.onClick.AddListener(() =>
        {
            uiManager.SetUIState(UIState.MainUI);
            Close();

            questManager.TriggerQuest(Quests.q_pinned);
        });

        introFormPanel.SetActive(true);
        introTextPanel.SetActive(false);
        
        ApplicationManager.StartScreenTextDataAvailable += UpdateText;
        ApplicationManager.LanguageChanged += UpdateText;
    }
    
    private void UpdateText()
    {
        foreach (var textElement in startScreenTextElements)
        {
            textElement.label.text = ApplicationManager.startScreenData[textElement.id];
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
        ApplicationManager.StartScreenTextDataAvailable -= UpdateText;
        ApplicationManager.LanguageChanged -= UpdateText;
    }
}

[Serializable]
public struct StartScreenTextElement
{
    public string id;
    public TextMeshProUGUI label;
}