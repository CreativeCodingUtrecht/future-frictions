using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class Drone : MonoBehaviour
{
    public bool Selected { get; private set; }

    [SerializeField]
    private EnvironmentChanges environmentChanges;

    [SerializeField]
    private DroneAnimation droneAnimation;

    [SerializeField]
    private DroneSelection droneSelection;

    [SerializeField]
    private QuestionScreen questionScreen;

    [SerializeField]
    private GameObject question;

    [SerializeField]
    private GameObject answers;

    [SerializeField]
    private DroneHighlightObject[] droneHighlightObjects;

    [SerializeField]
    private ScenarioOutcome[] scenarioOutcomes;

    [SerializeField]
    private AudioClip audioClip;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private Sprite[] questionImages;

    [SerializeField]
    private GameObject pinnedQuest;
    
    private bool selectionMade;

    private QuestManager questManager;
    private UIManager uiManager;

    private void Awake()
    {
        questManager = FindObjectOfType<QuestManager>();
        uiManager = FindObjectOfType<UIManager>();

        questionScreen.Close();
        SetOutcomeState(ScenarioOptions.NONE);
    }

    public void ResetDrone()
    {
        selectionMade = false;
        Selected = false;
        droneSelection.Reset();
        SetOutcomeState(ScenarioOptions.NONE);
    }

    public void SetSelectedOption(ScenarioOptions option)
    {
        SetFinished();

        environmentChanges.SetActiveScenario(Scenarios.Drone);
        environmentChanges.ActiveScenario.SetSelectedOption(option);

        SetOutcomeState(option);

        questManager.TriggerQuest(Quests.q_drone_1);
    }

    public void SetSelectedState(bool selected)
    {
        if (!selectionMade && !Selected)
        {
            questionScreen.SetTextData("drone_s_1", questionImages);
            questionScreen.Open();
            question.SetActive(false);
            answers.SetActive(false);

            //SetupButtons();

            droneAnimation.SetSelectedState(selected, () => { StartSequence(); });

            Selected = selected;
        }
    }

    public void StartSequence()
    {
        pinnedQuest.SetActive(false);
        
        Queue<Action> droneQueue = new Queue<Action>();

        for (int i = 0; i < droneHighlightObjects.Length; i++)
        {
            var dho = droneHighlightObjects[i];

            droneQueue.Enqueue(() =>
            {
                dho.HandleHighlight(droneQueue);
                audioSource.PlayOneShot(audioClip);
            });
        }

        droneQueue.Enqueue(() =>
        {
            StartCoroutine(WaitAndHandleNext(() =>
            {
                question.SetActive(true);

                var nextAction = droneQueue.Dequeue();
                nextAction?.Invoke();
            }));
        });

        droneQueue.Enqueue(() =>
        {
            StartCoroutine(WaitAndHandleNext(() =>
            {
                answers.SetActive(true);
            }));
        });

        var nextAction = droneQueue.Dequeue();
        nextAction?.Invoke();
    }

    private void SetFinished()
    {
        selectionMade = true;

        questionScreen.Close();
        droneAnimation.RunAnimation();
        droneSelection.SetFinished();
    }

    //private void SetupButtons()
    //{
    //    foreach (var btn in selectionButtons)
    //    {
    //        btn.button.onClick.RemoveAllListeners();
    //        btn.button.onClick.AddListener(() => { SetSelectedOption(btn.option); });

    //        btn.button.GetComponent<ButtonHover>().enabled = true;
    //    }
    //}

    private IEnumerator WaitAndHandleNext(Action nextAction)
    {
        yield return new WaitForSeconds(2f);

        nextAction?.Invoke();
    }

    private void SetOutcomeState(ScenarioOptions option)
    {
        foreach (var outcome in scenarioOutcomes)
        {
            if (outcome.option == option)
            {
                outcome.Activate();
            }
            else
            {
                outcome.Hide();
            }
        }
    }
}

[Serializable]
public struct OptionSelectionButton
{
    public Button button;
    public ScenarioOptions option;
}