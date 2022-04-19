using UnityEngine;

public class ScenarioSelection : MonoBehaviour
{
    [SerializeField]
    private EnvironmentChanges environmentChanges;

    [SerializeField]
    private int scenarioID;

    [SerializeField]
    private int optionID;

    private bool isSelected;

    private void OnMouseDown()
    {
        environmentChanges.scenarios[scenarioID].Options[0].SetSelected(true);
    }
}