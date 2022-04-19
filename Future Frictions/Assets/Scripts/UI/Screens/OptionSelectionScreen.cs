using UnityEngine;
using UnityEngine.UI;

public class OptionSelectionScreen : BaseScreen
{
    [SerializeField]
    private Button selectButton;

    [SerializeField]
    private Button backButton;

    public void Initialize(OptionSelection currentOption)
    {
        selectButton.onClick.AddListener(() =>
        {
            currentOption.SetOptionSelectedState(true);
        });

        backButton.onClick.AddListener(() => { currentOption.Close(); });
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}