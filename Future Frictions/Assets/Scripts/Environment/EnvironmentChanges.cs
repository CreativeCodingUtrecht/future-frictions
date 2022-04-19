using UnityEngine;
using System;
using UnityEngine.Events;
using System.Linq;

public class EnvironmentChanges : MonoBehaviour
{
    public UnityEvent onAllSelectionsMade;

    public event Action<Scenarios, ScenarioOptions> OnSelectionMade;

    public static EnvironmentChanges Instance;

    public Scenario ActiveScenario { get; private set; }

    public Scenario[] scenarios;

    private void Awake()
    {
        Instance = this;

        foreach (var scenario in scenarios)
        {
            scenario.OnOptionSelected += OnOptionSelected;
        }
    }

    public void ResetData()
    {
        SetScenariosInactive();
        OnOptionSelected(ScenarioOptions.NONE);
        ActiveScenario = null;
    }

    private void OnOptionSelected(ScenarioOptions optionSelection)
    {
        // Check if all selections are made
        onAllSelectionsMade?.Invoke();

        OnSelectionMade?.Invoke(ActiveScenario.ScenarioID, optionSelection);
    }

    public void SetActiveScenario(Scenarios scenario)
    {
        for (int i = 0; i < scenarios.Length; i++)
        {
            scenarios[i].SetActiveState(scenarios[i].ScenarioID == scenario);
        }

        ActiveScenario = scenarios[(int)scenario];
    }

    public void SetScenariosInactive()
    {
        for (int i = 0; i < scenarios.Length; i++)
        {
            scenarios[i].SetActiveState(false);
        }
    }

    public void ActivateScenarioVariations()
    {
        foreach (var scenario in scenarios)
        {
            var scenarioOption = scenario.Options.Where(x => x.ScenarioOptionID == scenario.SelectedOption).FirstOrDefault();
            if (scenarioOption != null && scenarioOption.Variations)
            {
                scenarioOption.Variations.Show();
            }
        }
    }
}

[Serializable]
public class Scenario
{
    public ScenarioOptions SelectedOption { get; private set; } = ScenarioOptions.NONE;
    public bool Active { get; private set; }

    public string Name;
    public Scenarios ScenarioID;
    public ScenarioOption[] Options;

    public event Action<ScenarioOptions> OnOptionSelected;

    public void SetActiveState(bool active)
    {
        Active = active;
    }

    public void SetSelectedOption(ScenarioOptions scenarioOption)
    {
        for (int i = 0; i < Options.Length; i++)
        {
            Options[i].SetSelected(Options[i].ScenarioOptionID == scenarioOption);
        }

        SelectedOption = scenarioOption;

        OnOptionSelected?.Invoke(scenarioOption);
    }
}

[Serializable]
public class ScenarioOption
{
    public string Name;
    public ScenarioOptions ScenarioOptionID = ScenarioOptions.NONE;
    public bool Selected;

    public ScenarioVariations Variations;

    public void SetSelected(bool selected)
    {
        Selected = selected;
    }
}