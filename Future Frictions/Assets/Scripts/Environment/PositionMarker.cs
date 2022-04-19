using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PositionMarker : MonoBehaviour
{
    public enum Mode
    {
        Default,
        Highlighted
    }

    [SerializeField]
    private List<Renderer> myRenderers = new List<Renderer>();

    [SerializeField]
    private Color highlightedColor;

    private Color normalColor;

    private void Awake()
    {
        if (!myRenderers.Any())
        {
            Debug.Log("No renderers in list myRenderers");
            return;
        }
        normalColor = myRenderers[0].material.color;
    }

    private void OnMouseEnter()
    {
        if (ApplicationManager.WorldInteractions)
        {
            SetColorState(Mode.Highlighted);
        }
    }

    private void OnMouseExit()
    {
        SetColorState(Mode.Default);
    }

    private void SetColorState(Mode mode)
    {
        switch (mode)
        {
            case Mode.Default:
                foreach (Renderer myRenderer in myRenderers)
                {
                    myRenderer.material.color = normalColor;
                }
                break;

            case Mode.Highlighted:
                foreach (Renderer myRenderer in myRenderers)
                {
                    myRenderer.material.color = highlightedColor;
                }
                break;

            default:
                break;
        }
    }
}