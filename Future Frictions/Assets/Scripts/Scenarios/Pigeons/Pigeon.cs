using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pigeon : MonoBehaviour
{
    public bool Selected { get; private set; }

    [SerializeField] private EnvironmentChanges environmentChanges;

    [SerializeField] private PigeonAnimation pigeonAnimation;

    [SerializeField] private PigeonSelection pigeonSelection;

    [SerializeField] private QuestionScreen questionScreen;

    [SerializeField] private GameObject question;

    [SerializeField] private GameObject answers;

    [SerializeField] private ScenarioOutcome[] scenarioOutcomes;

    [SerializeField]
    private Sprite[] questionImages;

    private bool selectionMade;

    private QuestManager questManager;

    private void Awake()
    {
        questManager = FindObjectOfType<QuestManager>();

        questionScreen.Close();
        SetOutcomeState(ScenarioOptions.NONE);
    }

    public void ResetDrone()
    {
        selectionMade = false;
        Selected = false;
        pigeonSelection.Reset();
        SetOutcomeState(ScenarioOptions.NONE);
    }

    public void SetSelectedOption(ScenarioOptions option)
    {
        SetFinished();

        environmentChanges.SetActiveScenario(Scenarios.Pigeon);
        environmentChanges.ActiveScenario.SetSelectedOption(option);

        SetOutcomeState(option);

        questManager.TriggerQuest(Quests.q_pigeon_2);
    }

    public void SetSelectedState(bool selected)
    {
        if (!selectionMade && !Selected)
        {
            questionScreen.SetTextData("pigeons_s_1", questionImages);
            questionScreen.Open();
            question.SetActive(false);
            answers.SetActive(false);

            pigeonAnimation.SetSelectedState(selected, () => { StartSequence(); });

            Selected = selected;
        }
    }

    public void StartSequence()
    {
        question.SetActive(true);
        answers.SetActive(true);
    }

    private void SetFinished()
    {
        selectionMade = true;

        questionScreen.Close();
        pigeonAnimation.RunAnimation();
        pigeonSelection.SetFinished();
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
