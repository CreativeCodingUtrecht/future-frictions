using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostcardScreen : BaseScreen
{
    //[SerializeField]
    //private TMP_Text nameText;

    //[SerializeField]
    //private TMP_Text locationAndDateText;

    //[SerializeField]
    //private TMP_InputField otherNameInput;

    //[SerializeField]
    //private TMP_Text otherNameText;

    //[SerializeField]
    //private Transform textInputGroup;

    [SerializeField]
    private TMP_InputField emailInput;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Button sendButton;

    [SerializeField]
    private DataController dataController;

    [SerializeField]
    private Transform contentGroup;

    [Header("Prefabs")]
    [SerializeField]
    private TMP_Text labelPrefab;

    [SerializeField]
    private TMP_InputField inputPrefab;

    private TMP_InputField[] textInputFields;

    private List<object> textList = new List<object>();

    private void Awake()
    {
        var postcardText = ApplicationManager.questTextData["postcard"];

        var contentWidth = (contentGroup.transform as RectTransform).rect.width;

        postcardText = postcardText.Replace("[place]", "Amersfoort");
        postcardText = postcardText.Replace("[data]", DateTime.Now.Date.ToString("dd-MM-yyyy"));
        postcardText = postcardText.Replace("[name]", $"\n{ApplicationManager.Instance.userData.Name}");

        var textLines = postcardText.Split('\n');

        foreach (var line in textLines)
        {
            var inputLines = line.Split('_');

            if (inputLines.Length > 1)
            {
                for (int i = 0; i < inputLines.Length; i++)
                {
                    var label = Instantiate(labelPrefab, contentGroup);
                    label.text = inputLines[i];

                    label.ForceMeshUpdate();
                    var val = label.GetRenderedValues();

                    if (string.IsNullOrEmpty(inputLines[i]))
                    {
                        var layoutElement = label.GetComponent<LayoutElement>();
                        layoutElement.preferredWidth = contentWidth;
                    }
                    else if (val.y > contentWidth)
                    {
                        var layoutElement = label.GetComponent<LayoutElement>();
                        layoutElement.preferredWidth = contentWidth;
                    }

                    textList.Add(label);

                    if (i < inputLines.Length - 1)
                    {
                        var input = Instantiate(inputPrefab, contentGroup);

                        (input.transform as RectTransform).sizeDelta = new Vector2((input.transform as RectTransform).sizeDelta.x, 20);

                        textList.Add(input);
                    }
                }
            }
            else
            {
                var label = Instantiate(labelPrefab, contentGroup);
                label.text = line;

                label.ForceMeshUpdate();
                var val = label.GetRenderedValues();

                if (string.IsNullOrEmpty(line))
                {
                    var layoutElement = label.GetComponent<LayoutElement>();
                    layoutElement.preferredWidth = contentWidth;
                }
                else if (val.y > contentWidth)
                {
                    var layoutElement = label.GetComponent<LayoutElement>();
                    layoutElement.preferredWidth = contentWidth;
                }

                textList.Add(label);
            }
        }

        Canvas.ForceUpdateCanvases();
    }

    private void Start()
    {
        UIManager.Instance.CloseDialog();

        //nameText.text = ApplicationManager.Instance.userData.Name;
        //locationAndDateText.text = $"Amersfoort / {DateTime.Now.Date.ToString("dd-MM-yyyy")}";

        //otherNameInput.onValueChanged.AddListener((value) => { otherNameText.text = value; });

        //textInputFields = textInputGroup.GetComponentsInChildren<TMP_InputField>();

        backButton.onClick.AddListener(() =>
        {
            UIManager.Instance.SetUIState(UIState.MainUI);
            Close();
        });

        sendButton.onClick.AddListener(() =>
        {
            sendButton.interactable = false;

            var message = GetMessage();

            dataController.CreateAndStoreData(emailInput.text, "Future Frictions Email Test", message, (success) =>
            {
                if (success)
                {
                    sendButton.interactable = true;
                    UIManager.Instance.SetUIState(UIState.EndScreen);
                    Close();
                }
            });
        });
    }

    private string GetMessage()
    {
        string message = "";

        // Go trough all elements and combine
        foreach (var t in textList)
        {
            if (t is TMP_Text)
            {
                var te = (t as TMP_Text).text;

                if (string.IsNullOrEmpty(te))
                {
                    message += "\n\n";
                }
                else
                {
                    message += te;
                }
            }
            else if (t is TMP_InputField)
            {
                var te = (t as TMP_InputField).text;

                if (string.IsNullOrEmpty(te))
                {
                    message += "_";
                }
                else
                {
                    message += te;
                }
            }
        }

        return message;
    }
}