using UnityEngine;

public class Cat : MonoBehaviour
{
    public bool Selected { get; private set; }

    [SerializeField] private EnvironmentChanges environmentChanges;

    [SerializeField] private CatAnimation catAnimation;

    [SerializeField] private CatSelection catSelection;

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
        catSelection.Reset();
        SetOutcomeState(ScenarioOptions.NONE);
    }

    public void SetSelectedOption(ScenarioOptions option)
    {
        SetFinished();

        environmentChanges.SetActiveScenario(Scenarios.Cat);
        environmentChanges.ActiveScenario.SetSelectedOption(option);

        SetOutcomeState(option);

        questManager.TriggerQuest(Quests.q_cat_2);
    }

    public void SetSelectedState(bool selected)
    {
        if (!selectionMade && !Selected)
        {
            questionScreen.SetTextData("cat_s_1", questionImages);
            questionScreen.Open();
            question.SetActive(false);
            answers.SetActive(false);

            catAnimation.SetSelectedState(selected, () => { StartSequence(); });

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
        catAnimation.RunAnimation();
        catSelection.SetFinished();
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