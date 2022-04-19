using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Material HoverMaterial;
    public List<GameObject> Stencils = new List<GameObject>();

    [SerializeField]
    private EnvironmentChanges environmentChanges;

    [SerializeField]
    private Scenarios scenario;

    [SerializeField]
    private GameObject positionsObject;

    private Material originalMaterial;

    private bool stencilsAreVisible;
    private bool isInteractable = true;

    private void Start()
    {
        originalMaterial = gameObject.GetComponent<Renderer>().material;

        if (gameObject.GetComponent<BoxCollider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
        }

        HideStencils();
    }

    public void SetFinished()
    {
        isInteractable = false;
        HideStencils();
    }

    private void OnMouseEnter()
    {
        if (!isInteractable)
            return;

        gameObject.GetComponent<Renderer>().material = HoverMaterial;
    }

    private void OnMouseExit()
    {
        if (!isInteractable)
            return;

        gameObject.GetComponent<Renderer>().material = originalMaterial;
    }

    private void OnMouseDown()
    {
        if (!isInteractable)
            return;

        if (!stencilsAreVisible)
        {
            ShowStencils();
            stencilsAreVisible = true;
        }
        else
        {
            HideStencils();
            stencilsAreVisible = false;
        }
    }

    private void ShowStencils()
    {
        if (Stencils.Count != 0)
        {
            environmentChanges.SetActiveScenario(scenario);
            positionsObject.SetActive(false);

            foreach (GameObject stencil in Stencils)
            {
                stencil.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.Log("No stencils to show for: " + gameObject.name);
        }
    }

    private void HideStencils()
    {
        if (Stencils.Count != 0)
        {
            environmentChanges.SetScenariosInactive();
            positionsObject.SetActive(true);

            foreach (GameObject stencil in Stencils)
            {
                stencil.gameObject.SetActive(false);
            }
        }
    }
}