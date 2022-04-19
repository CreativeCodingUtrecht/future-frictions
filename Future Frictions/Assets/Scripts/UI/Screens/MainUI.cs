using UnityEngine;
using UnityEngine.UI;

public class MainUI : BaseScreen
{
    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private Button finishButton;

    private void Awake()
    {
        finishButton.onClick.AddListener(() =>
        {
            Close();
            uiManager.SetUIState(UIState.Postcard);
        });

        finishButton.gameObject.SetActive(false);

        //EnvironmentChanges.Instance.onAllSelectionsMade.AddListener(AllSelectionsMade);
    }

    public void AllSelectionsMade()
    {
        finishButton.gameObject.SetActive(true);
    }
}