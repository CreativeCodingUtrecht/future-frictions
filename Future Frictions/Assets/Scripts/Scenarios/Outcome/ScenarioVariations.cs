using UnityEngine;
using UnityEngine.UI;

public class ScenarioVariations : MonoBehaviour
{
    [SerializeField]
    private GameObject[] variations;

    [SerializeField]
    private Slider slider;

    private void Start()
    {
        SetVariation(slider.value);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetVariation(float index)
    {
        for (int i = 0; i < variations.Length; i++)
        {
            variations[i].SetActive(i == index);
        }
    }
}