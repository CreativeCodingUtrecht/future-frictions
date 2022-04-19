using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TextLanguageChanger : MonoBehaviour
    {
        [SerializeField]
        private string englishText;
        
        [SerializeField]
        private string dutchText;

        private TextMeshProUGUI textElement;
        
        private void Awake()
        {
            textElement = GetComponent<TextMeshProUGUI>();

            if (textElement == null)
            {
                Debug.LogError("Text element not found", gameObject);
                return;
            }

            DataManager.AppTextAvailable += LanguageChanged;
            DataManager.LanguageChanged += LanguageChanged;
        }

        private void LanguageChanged()
        {
            textElement.text = DataManager.CurrentLanguage switch
            {
                "en-EN" => englishText,
                "nl-NL" => dutchText,
                _ => textElement.text
            };
        }

        private void OnDestroy()
        {
            DataManager.AppTextAvailable -= LanguageChanged;
            DataManager.LanguageChanged -= LanguageChanged;
        }
    }
}