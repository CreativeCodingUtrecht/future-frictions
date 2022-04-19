using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Transform uiRoot;

    [SerializeField]
    private EnvironmentChanges environmentChanges;

    [Header("Screens")]
    [SerializeField]
    private IntroScreen introScreen;

    [SerializeField]
    private MainUI mainUI;

    [SerializeField]
    private OptionSelectionScreen optionSelectionScreen;

    [SerializeField]
    private PostcardScreen postcardScreen;

    [SerializeField]
    private DialogUI dialogUI;

    [SerializeField]
    private Popup popupScreen;

    [SerializeField]
    private EndScreen endScreen;

    private void Awake()
    {
        Instance = this;
        SetUIState(UIState.IntroForm);
    }

    public BaseScreen SetUIState(UIState uiState)
    {
        switch (uiState)
        {
            case UIState.IntroForm:
                introScreen.Open();
                ApplicationManager.SetWorldInteractionsState(false);
                return introScreen;

            case UIState.MainUI:
                mainUI.Open();
                ApplicationManager.SetWorldInteractionsState(true);
                return mainUI;

            case UIState.OptionSelection:
                optionSelectionScreen.Open();
                ApplicationManager.SetWorldInteractionsState(false);
                return optionSelectionScreen;

            case UIState.Postcard:
                postcardScreen.Open();
                environmentChanges.ActivateScenarioVariations();
                ApplicationManager.SetWorldInteractionsState(false);
                return postcardScreen;

            case UIState.Dialog:
                dialogUI.Open();
                return dialogUI;

            case UIState.EndScreen:
                endScreen.Open();
                return endScreen;

            default:
                return null;
        }
    }

    public void ShowPopup(string text, Action firstAction, Action secondAction)
    {
        popupScreen.Init(text, firstAction, secondAction);
    }

    public void CloseDialog()
    {
        dialogUI.Close();
    }
}