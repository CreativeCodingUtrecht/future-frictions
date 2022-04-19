using System.Collections.Generic;
using UnityEngine;

public class ToggleIcon : MonoBehaviour
{
    public Material HoverMaterial;

    //This could be a list of icons, or we could replace the icon from somewhere else
    public GameObject Icon; 
    private Material originalMaterial;

    private bool iconIsVisble;
    private bool isInteractable = true;

    private void Start()
    {
        originalMaterial = gameObject.GetComponent<Renderer>().material;

        if (gameObject.GetComponent<BoxCollider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
        }

        HideIcon();
    }

    public void SetFinished()
    {
        isInteractable = false;
        HideIcon();
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

        if (!iconIsVisble)
        {
            ShowIcon();
            iconIsVisble = true;
        }
        else
        {
            HideIcon();
            iconIsVisble = false;
        }
    }

    private void ShowIcon()
    {
        
        Icon.gameObject.SetActive(true);

    }

    private void HideIcon()
    {
        Icon.gameObject.SetActive(false);

    }
}