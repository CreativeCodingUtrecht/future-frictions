using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class NextButton : MonoBehaviour
{
    [SerializeField]
    private Color inactiveColor;
    
    [SerializeField]
    private Color activeColor;
    
    [SerializeField]
    private Image icon;

    public void SetColor(bool interactable)
    {
        icon.color = interactable ? activeColor : inactiveColor;
    }
}
