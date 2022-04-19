using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.Events;

[Serializable]
public enum Quests
{
    None,
    q_pinned = 1,
    q_drone_1 = 2,
    q_drone_2 = 3,
    q_cat_1 = 4,
    q_cat_2 = 5,
    q_bulletin = 6,
    q_done = 7,
    q_pigeon_1 = 8,
    q_pigeon_2 = 9,
    q_cat_3 = 10
}

/// <summary>
/// Quests:
/// 1. Show prompt
/// 2. Activate Drone Scenario and Interact with drone people
/// 3. Activate Drone
/// 4. Interact with drone people
/// 5. Deactivate Drone Scenario -> Start Cat Scenario and Interact with cat people
/// 6. Activate Cat
/// 7. Interact with cat people
/// 8. Deactivate Cat Scenario -> Activate postcard
/// </summary>
public class QuestManager : MonoBehaviour
{
    public QuestData[] questData;

    [SerializeField] private GameObject pinnedQuest;

    [Space(20)] [SerializeField] private InteractableCharacter[] droneCharacters;

    [SerializeField] private InteractableCharacter[] catCharacters;
    
    [SerializeField] private InteractableCharacter[] pigeonCharacters;

    [SerializeField]
    private GameObject droneObject;

    [SerializeField]
    private GameObject catObject;
    
    [SerializeField]
    private GameObject pigeonObject;

    private DroneSelection droneSelection;
    private CatSelection catSelection;
    private PigeonSelection pigeonSelection;

    private UIManager uiManager;
    private MainUI mainUI;

    private bool droneActivated;
    private bool catActivated;
    private bool pigeonActivated;

    private float defaultDelay = 20f;

    private List<InteractableCharacters> interactedCharacters = new List<InteractableCharacters>();

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        mainUI = FindObjectOfType<MainUI>(true);
        droneSelection = FindObjectOfType<DroneSelection>();
        catSelection = FindObjectOfType<CatSelection>();
        pigeonSelection = FindObjectOfType<PigeonSelection>();
    }

    private void Start()
    {
        foreach (var character in catCharacters)
        {
            character.gameObject.SetActive(false);
        }

        foreach (var character in pigeonCharacters)
        {
            character.gameObject.SetActive(false);
        }

        catObject.SetActive(false);
        pigeonObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.RightAlt) && Input.GetKeyDown(KeyCode.B))
        {
            TriggerQuest(Quests.q_bulletin);
        }
    }

    public void ResetQuests()
    {
        foreach (QuestData quest in questData)
        {
            if (quest.questID != Quests.q_pinned)
            {
                quest.Completed = false;
            }
        }

        droneActivated = false;
        ActivateDrone();
        interactedCharacters.Clear();
    }

    public void ActivateDrone()
    {
        droneSelection.SetSelectableState(true);
        droneActivated = true;
    }

    public void ActivateCat()
    {
        // Debug.Log("Activate Cat");

        catSelection.SetSelectableState(true);
        catActivated = true;
    }

    public void ActivatePigeon()
    {
        pigeonSelection.SetSelectableState(true);
        pigeonActivated = true;
    }

    public void UpdateInteractions(InteractableCharacters character)
    {
        if (interactedCharacters.Contains(character)) return;

        interactedCharacters.Add(character);

        switch (EnvironmentChanges.Instance.ActiveScenario.ScenarioID)
        {
            case Scenarios.Drone:
                {
                    if (interactedCharacters.Count == droneCharacters.Length)
                    {
                        if (!droneActivated)
                        {
                            ActivateDrone();
                            interactedCharacters.Clear();
                        }
                        else
                        {
                            interactedCharacters.Clear();
                            StartCoroutine(TriggerDelayed(defaultDelay, Quests.q_drone_2));
                        }
                    }
                    break;
                }
            case Scenarios.Cat:
                {
                    if (interactedCharacters.Count == catCharacters.Length)
                    {
                        if (!catActivated)
                        {
                            StartCoroutine(TriggerDelayed(defaultDelay, Quests.q_cat_1));
                        }
                        else
                        {
                            interactedCharacters.Clear();
                            StartCoroutine(TriggerDelayed(defaultDelay, Quests.q_cat_3));
                        }
                    }
                    break;
                }
            case Scenarios.Pigeon:
            {
                if (interactedCharacters.Count == pigeonCharacters.Length)
                {
                    if (!pigeonActivated)
                    {
                        StartCoroutine(TriggerDelayed(defaultDelay, Quests.q_pigeon_1));
                    }
                    else
                    {
                        interactedCharacters.Clear();
                        StartCoroutine(TriggerDelayed(defaultDelay, Quests.q_bulletin));
                    }
                }
                break;
            }
        }
    }

    private IEnumerator PresentOptions()
    {
        yield return new WaitForSeconds(12f);

        uiManager.ShowPopup(
          "Are you curious about how the future would change if you chose another option? Go back and change what the drone does with the images!",
          () =>
          {
              var applicationManager = FindObjectOfType<ApplicationManager>();
              applicationManager.ResetScenarios();
          }, () => { TriggerQuest(Quests.q_bulletin); });
    }

    private IEnumerator TriggerDelayed(float waitTIme, Quests questID)
    {
        yield return new WaitForSeconds(waitTIme);
        TriggerQuest(questID);
    }

    public void TriggerQuest(Quests questID)
    {
        // Debug.Log($"Start quest -> {questID}");

        var qD = questData.FirstOrDefault(x => x.questID == questID);

        if (qD == null) return;
        if (qD.Completed)
        {
            return;
        }

        switch (questID)
        {
            case Quests.q_drone_2:
                foreach (var character in catCharacters)
                {
                    character.gameObject.SetActive(true);
                }

                catObject.SetActive(true);
                droneObject.SetActive(false);

                SetCatCharactersUpdated();
                break;
            case Quests.q_cat_3:
                foreach (var character in pigeonCharacters)
                {
                    character.gameObject.SetActive(true);
                }

                pigeonObject.SetActive(true);
                catObject.SetActive(false);

                StartCoroutine(WaitToActivate());
                break;
            case Quests.q_bulletin:
                pigeonObject.SetActive(false);
                SetPigeonCharactersDeactivated();
                break;
        }

        var dialogUI = (DialogUI) uiManager.SetUIState(UIState.Dialog);
        dialogUI.SetData(qD.GetQuestText(), qD.QuestImage, Color.white);

        qD.Completed = true;
        qD.onQuestStart?.Invoke();
    }

    private IEnumerator WaitToActivate()
    {
        yield return new WaitForSeconds(.5f);
        SetPigeonCharactersUpdated();
    }

    public void ShowPinnedQuest()
    {
        EnvironmentChanges.Instance.SetActiveScenario(Scenarios.Drone);

        foreach (var droneCharacter in droneCharacters)
        {
            droneCharacter.SetInteractionState(InteractionState.New);
        }

        StartCoroutine(ShowDelayed(5, () =>
        {
            pinnedQuest.SetActive(true);
            pinnedQuest.GetComponentInChildren<TextMeshProUGUI>().text = ApplicationManager.questTextData["q_pinned"];
        }));
    }

    public void ActivateTheCat()
    {
        ActivateCat();
        interactedCharacters.Clear();
    }
    
    public void ActivateThePigeon()
    {
        ActivatePigeon();
        interactedCharacters.Clear();
    }

    public void DroneScenarioDone()
    {
        EnvironmentChanges.Instance.SetActiveScenario(Scenarios.Cat);

        SetDroneCharactersDeactivated();

        // Activate cat interactable characters
        foreach (var catCharacter in catCharacters)
        {
            catCharacter.SetInteractionState(InteractionState.New);
        }
    }

    public void CatScenarioDone()
    {
        EnvironmentChanges.Instance.SetActiveScenario(Scenarios.Pigeon);

        SetCatCharactersDeactivated();

        // Activate cat interactable characters
        foreach (var pigeonCharacter in pigeonCharacters)
        {
            pigeonCharacter.SetInteractionState(InteractionState.New);
        }
    }

    public void AllQuestDone()
    {
        // Disable all drone interactable characters
        SetDroneCharactersDeactivated();

        // Activate cat interactable characters
        SetCatCharactersDeactivated();
        
        SetPigeonCharactersDeactivated();

        StartCoroutine(ShowDelayed(4, () => { mainUI.AllSelectionsMade(); }));
    }

    public void SetDroneCharactersUpdated()
    {
        foreach (var droneCharacter in droneCharacters)
        {
            droneCharacter.SetInteractionState(InteractionState.Updated);
        }
    }

    private void SetDroneCharactersDeactivated()
    {
        foreach (var droneCharacter in droneCharacters)
        {
            droneCharacter.SetInteractionState(InteractionState.NONE);
        }
    }

    public void SetCatCharactersUpdated()
    {
        foreach (var catCharacter in catCharacters)
        {
            catCharacter.SetInteractionState(InteractionState.Updated);
        }
    }

    private void SetCatCharactersDeactivated()
    {
        foreach (var catCharacter in catCharacters)
        {
            catCharacter.SetInteractionState(InteractionState.NONE);
        }
    }
    
    public void SetPigeonCharactersUpdated()
    {
        foreach (var pigeonCharacter in pigeonCharacters)
        {
            pigeonCharacter.SetInteractionState(InteractionState.Updated);
        }
    }

    private void SetPigeonCharactersDeactivated()
    {
        foreach (var pigeonCharacter in pigeonCharacters)
        {
            pigeonCharacter.SetInteractionState(InteractionState.NONE);
        }
    }

    private IEnumerator ShowDelayed(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback?.Invoke();
    }
}

[Serializable]
public class QuestData
{
    public Quests questID;

    public Sprite QuestImage;

    public bool Completed;

    public UnityEvent onQuestStart;

    public string GetQuestText()
    {
        return ApplicationManager.questTextData[questID.ToString()];
    }
}