using UnityEngine;

public class DroneSelection : MonoBehaviour
{
    public Color defaultColor;
    public Color hoverColor;
    public Color selectedColor;

    [Range(0, 100)]
    public int thickness = 12;

    [SerializeField]
    private float interactionDistance;

    [SerializeField]
    private Drone drone;

    [SerializeField]
    private Outline outline;

    private bool selectable;

    private bool selected;
    private bool finished;

    private void Start()
    {
        SetSelectableState(false);

        outline.SetOutlineThickness(thickness);
        outline.SetOutlineColor(defaultColor);
    }

    public void Reset() {
        finished = false;
        selected = false;

        SetSelectableState(false);

        outline.SetOutlineColor(defaultColor);
    }

    public void SetSelectableState(bool state)
    {
        selectable = state;

        outline.SetOutlineEnabled(state);
    }

    public void SetFinished()
    {
        finished = true;
        outline.SetOutlineEnabled(false);
    }

    private void OnMouseEnter()
    {
        if (finished || !selectable)
            return;

        if (ApplicationManager.WorldInteractions && !selected && Vector3.Distance(transform.position, Player.current.transform.position) < interactionDistance)
        {
            //outline.SetOutlineEnabled(true);
            outline.SetOutlineColor(hoverColor);
        }
    }

    private void OnMouseExit()
    {
        if (finished || !selectable)
            return;

        if (ApplicationManager.WorldInteractions && !selected)
        {
            //outline.SetOutlineEnabled(false);
            outline.SetOutlineColor(defaultColor);
        }
    }

    private void OnMouseDown()
    {
        if (finished || !selectable)
            return;

        if (ApplicationManager.WorldInteractions && Vector3.Distance(transform.position, Player.current.transform.position) < interactionDistance)
        {
            selected = true;
            drone.SetSelectedState(selected);
            outline.SetOutlineColor(selectedColor);
        }
    }
}