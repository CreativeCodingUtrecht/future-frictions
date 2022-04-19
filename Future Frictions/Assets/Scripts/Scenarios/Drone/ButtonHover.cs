using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject visualObject;

    private void Awake()
    {
        visualObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        visualObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        visualObject.SetActive(false);
    }

    private void OnDisable()
    {
        visualObject.SetActive(false);
    }
}