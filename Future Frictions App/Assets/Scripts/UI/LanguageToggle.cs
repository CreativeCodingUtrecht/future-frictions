using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LanguageToggle : MonoBehaviour
    {
        [SerializeField]
        private Button languageToggleButton;

        [SerializeField]
        private DataManager dataManager;

        [SerializeField]
        private Sprite[] flagSprites;

        [SerializeField]
        private string[] languageNames;

        [SerializeField]
        private TextMeshProUGUI textLabel;

        [SerializeField]
        private Image flagIcon;
        
        private readonly string[] languages = new string[2] { "nl-NL", "en-EN" };
        
        public void Start()
        {
            DataManager.AppTextAvailable += LanguageChanged;
            DataManager.LanguageChanged += LanguageChanged;
            
            languageToggleButton.onClick.AddListener(ToggleLanguage);
        }

        private void ToggleLanguage()
        {
            dataManager.ChangeCurrentLanguage(DataManager.CurrentLanguage == languages[1] ? languages[0] : languages[1]);
        }
        
        private void LanguageChanged()
        {
            if (DataManager.CurrentLanguage == languages[1])
            {
                flagIcon.sprite = flagSprites[1];
                textLabel.text = languageNames[1];
            }
            else if (DataManager.CurrentLanguage == languages[0])
            {
                flagIcon.sprite = flagSprites[0];
                textLabel.text = languageNames[0];
            }
        }

        private void OnDestroy()
        {
            DataManager.AppTextAvailable -= LanguageChanged;
            DataManager.LanguageChanged -= LanguageChanged;
        }
    }
}