using UnityEngine;
using UnityEngine.Events;

public class ScenarioOutcome : MonoBehaviour
{
    public ScenarioOptions option;

    public UnityEvent onOptionSelected;

    [SerializeField]
    private WorldObject[] objectsToHide;

    private WorldObject[] optionWorldObjects;

    private void Awake()
    {
        optionWorldObjects = transform.GetComponentsInChildren<WorldObject>();
    }

    public void Activate()
    {
        foreach (var worldObject in objectsToHide)
        {
            worldObject.AnimateOut();
        }

        gameObject.SetActive(true);

        foreach (var worldObject in optionWorldObjects)
        {
            worldObject.AnimateIn();
        }

        onOptionSelected?.Invoke();
    }

    public void Hide()
    {
        gameObject.SetActive(false);

        foreach (var worldObject in objectsToHide)
        {
            worldObject.AnimateIn();
        }
    }
}