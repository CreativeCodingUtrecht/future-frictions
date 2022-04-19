using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreen : BaseScreen
{
    [SerializeField]
    private TextMeshProUGUI textArea;

    [SerializeField]
    private Button restartButton;

    private void Awake()
    {
        textArea.text = DataManager.otherTextData["endscreen"];

        restartButton.onClick.AddListener(() =>
        {
            AppManager.Instance.RestartApp();
        });
    }
}