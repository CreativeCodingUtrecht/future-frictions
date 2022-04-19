using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractableCharacter : MonoBehaviour
{
    public InteractableCharacters me;

    public ScenarioIDs myID;

    [SerializeField] private Outline outlineShader;

    //[SerializeField] private Image statusImageBackground;

    //[SerializeField] private Image statusImage;

    [SerializeField] private Sprite avatarImage;

    //[SerializeField] private TMP_Text statusText;

    [SerializeField]
    private StatusCube statusCube;

    private ScenarioTextData scenarioData;

    private EnvironmentChanges environmentChanges;
    private UIManager uiManager;
    private QuestManager questManager;

    private InteractionStateData currentStateData;

    private void Awake()
    {
        questManager = FindObjectOfType<QuestManager>();
        
        ApplicationManager.ScenarioTextDataAvailable += OnScenarioTextDataAvailable;
        ApplicationManager.LanguageChanged += OnScenarioTextDataAvailable;
    }

    private void Start()
    {
        environmentChanges = EnvironmentChanges.Instance;
        uiManager = UIManager.Instance;

        outlineShader.SetOutlineEnabled(true);

        SetInteractionState(InteractionState.NONE);
    }

    private void OnScenarioTextDataAvailable()
    {
        scenarioData = ApplicationManager.scenariosTextData[myID.ToString()];
    }

    private void SetCurrentDialog()
    {
        var dialogUI = (DialogUI)uiManager.SetUIState(UIState.Dialog);

        SetInteractionState(InteractionState.Read);
        
        if (scenarioData.id != myID.ToString())
            scenarioData = ApplicationManager.scenariosTextData[myID.ToString()];

        if (environmentChanges.ActiveScenario != null && environmentChanges.ActiveScenario.ScenarioID != Scenarios.NONE)
        {
            switch (environmentChanges.ActiveScenario.SelectedOption)
            {
                case ScenarioOptions.OptionA:
                    dialogUI.SetData(scenarioData.optionA, avatarImage != null ? avatarImage : currentStateData.Sprite, avatarImage != null ? default : currentStateData.Color);
                    break;

                case ScenarioOptions.OptionB:
                    dialogUI.SetData(scenarioData.optionB, avatarImage != null ? avatarImage : currentStateData.Sprite, avatarImage != null ? default : currentStateData.Color);
                    break;

                case ScenarioOptions.OptionC:
                    dialogUI.SetData(scenarioData.optionC, avatarImage != null ? avatarImage : currentStateData.Sprite, avatarImage != null ? default : currentStateData.Color);
                    break;

                case ScenarioOptions.NONE:
                    dialogUI.SetData(scenarioData.beforeText, avatarImage != null ? avatarImage : currentStateData.Sprite, avatarImage != null ? default : currentStateData.Color);
                    break;

                default:
                    break;
            }
        }
        else
        {
            dialogUI.SetData(scenarioData.beforeText, avatarImage != null ? avatarImage : currentStateData.Sprite, avatarImage != null ? default : currentStateData.Color);
        }
    }

    public void SetInteractionState(InteractionState interactionState)
    {
        currentStateData = GetStateData(interactionState);

        if (currentStateData == null)
        {
            return;
        }

        outlineShader.SetOutlineColor(currentStateData.Color);
        outlineShader.SetOutlineEnabled(true);

        statusCube.SetStatus(currentStateData);

        switch (interactionState)
        {
            case InteractionState.NONE:
                outlineShader.SetOutlineEnabled(false);
                break;

            default:
                break;
        }
    }

    private InteractionStateData GetStateData(InteractionState interactionState)
    {
        return ApplicationManager.Instance.stateData.Where(x => x.State == interactionState).FirstOrDefault();
    }

    private void OnMouseDown()
    {
        if (ApplicationManager.WorldInteractions && currentStateData.State != InteractionState.NONE && EnvironmentChanges.Instance != null && EnvironmentChanges.Instance.ActiveScenario != null)
        {
            questManager.UpdateInteractions(me);

            SetCurrentDialog();
        }
    }

    private void OnDestroy()
    {
        ApplicationManager.ScenarioTextDataAvailable -= OnScenarioTextDataAvailable;
        ApplicationManager.LanguageChanged -= OnScenarioTextDataAvailable;
    }
}

[System.Serializable]
public enum InteractableCharacters
{
    DEFAULT,
    STREETARTIST,
    OLDLADY,
    TEENAGER,
    Father,
    PolicyMaker,
    HealthWorker,
    ShopOwner,
    TechSpecialist,
    Visitor
}

public enum ScenarioIDs
{
    none,
    drone_s_1,
    drone_p_1,
    drone_p_2,
    drone_p_3,
    cat_s_1,
    cat_p_1,
    cat_p_2,
    cat_p_3,
    pigeons_s_1,
    pigeons_p_1,
    pigeons_p_2,
    pigeons_p_3
}