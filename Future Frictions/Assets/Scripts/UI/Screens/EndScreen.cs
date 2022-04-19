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
        textArea.text = ApplicationManager.questTextData["endscreen"];

        restartButton.onClick.AddListener(() =>
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        });
    }
}