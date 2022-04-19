using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ResultScreen : BaseScreen
{
    [SerializeField]
    private Button nextButton;

    [SerializeField]
    private ResultGroup[] resultGroups;

    private NextButton nB;
    
    private void Awake()
    {
        nextButton.onClick.AddListener(() => {
            AppManager.Instance.uiManager.SetScreen(Screens.QUESTION);
            var screen = AppManager.Instance.uiManager.GetScreen<QuestionScreen>();
            screen.InitializeQuestion();
        });
        
        nB = nextButton.GetComponent<NextButton>();
        nB.SetColor(false);
        nextButton.interactable = false;
    }

    public override void Open()
    {
        base.Open();

        foreach (var g in resultGroups)
        {
            g.gameObject.SetActive(false);
        }

        var data = AppManager.Instance.dataManager.CurrentScenario;
        var group = resultGroups.FirstOrDefault(x => x.scenario == data.Scenario);

        group.Activate(data.SelectedOption);

        StartCoroutine(ActivateButton());
    }

    private IEnumerator ActivateButton()
    {
        yield return new WaitForSeconds(5f);
        
        nextButton.interactable = true;
        
        if (nB)
        {
            nB.SetColor(true);
        }
    }
}