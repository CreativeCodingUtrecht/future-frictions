using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueScreen : MonoBehaviour
{
    [SerializeField]
    private Button closeButton;

    [SerializeField]
    private TMP_Text textArea;

    [SerializeField]
    private Image avatarImage;

    private void Awake()
    {
        closeButton.onClick.AddListener(() => {
            Hide();
        });
    }

    public void Initialize(string text, Sprite avatar = null)
    {
        textArea.text = text;

        if (avatar)
        {
            avatarImage.sprite = avatar;
        }

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
