using UnityEngine;

public class OptionSelection : MonoBehaviour
{
    [SerializeField]
    private EnvironmentChanges environmentChanges;

    [SerializeField]
    private Player player;

    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private Scenarios scenarioID;

    [SerializeField]
    private ScenarioOptions optiondID;

    private bool isActive;
    private bool isSelected;

    private Vector3 currentTarget;
    private Vector3 startPosition;

    private OptionSelectionScreen optionSelectionScreen;

    private void Awake()
    {
        startPosition = transform.position;
        currentTarget = startPosition;
    }

    private void OnMouseDown()
    {
        if (!isActive)
            Open();
    }

    public void Open()
    {
        SetActiveState(true);

        optionSelectionScreen = (OptionSelectionScreen)uiManager.SetUIState(UIState.OptionSelection);
        optionSelectionScreen.Initialize(this);
    }

    public void Close()
    {
        SetActiveState(false);

        if (optionSelectionScreen)
        {
            optionSelectionScreen.Close();
        }
    }

    private void Update()
    {
        if (Vector3.Distance(transform.parent.position, currentTarget) > 0.01f)
        {
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, currentTarget, 10 * Time.deltaTime);
        }
    }

    public void SetOptionSelectedState(bool active)
    {
        // Mark this option as selected or not selected
        isSelected = active;

        // Do stuff when the state changed
        environmentChanges.ActiveScenario.SetSelectedOption(optiondID);

        Close();
    }

    private void SetActiveState(bool active)
    {
        isActive = active;

        if (active)
        {
            currentTarget = player.playerCamera.transform.position;
            player.SetPointerState(false);
        }
        else
        {
            currentTarget = startPosition;
            player.SetPointerState(true);
        }
    }
}