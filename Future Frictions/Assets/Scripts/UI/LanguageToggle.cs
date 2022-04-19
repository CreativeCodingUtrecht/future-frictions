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
        private ApplicationManager applicationManager;

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
            ApplicationManager.ScenarioTextDataAvailable += LanguageChanged;
            ApplicationManager.LanguageChanged += LanguageChanged;
            
            languageToggleButton.onClick.AddListener(ToggleLanguage);
        }

        private void ToggleLanguage()
        {
            applicationManager.ChangeCurrentLanguage(ApplicationManager.CurrentLanguage == languages[1] ? languages[0] : languages[1]);
        }
        
        private void LanguageChanged()
        {
            if (ApplicationManager.CurrentLanguage == languages[1])
            {
                flagIcon.sprite = flagSprites[1];
                textLabel.text = languageNames[1];
            }
            else if (ApplicationManager.CurrentLanguage == languages[0])
            {
                flagIcon.sprite = flagSprites[0];
                textLabel.text = languageNames[0];
            }
        }

        private void OnDestroy()
        {
            ApplicationManager.ScenarioTextDataAvailable -= LanguageChanged;
            ApplicationManager.LanguageChanged -= LanguageChanged;
        }
    }
}