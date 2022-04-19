using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InteractableCharacter : MonoBehaviour
{
    [SerializeField]
    private InteractableCharacters character;

    [SerializeField]
    private Sprite avatar;

    [SerializeField]
    private IndicatorAnimation indicatorAnimation;

    [SerializeField]
    private FadeSequence fadeSequence;
    
    private void Start()
    {
        var fP = GetComponent<FadePanel>();
        if (!fP)
            indicatorAnimation.AnimateIn();
        
        var btn = GetComponent<Button>();
        btn.onClick.AddListener(HandleText);
    }

    private void HandleText()
    {
        var selectedOption = AppManager.Instance.dataManager.CurrentScenario.SelectedOption;
        var data = DataManager.scenariosTextData[character.ToString()];

        switch (selectedOption)
        {
            case Options.A:
                AppManager.Instance.dialogueScreen.Initialize(data.optionA, avatar);
                break;
            case Options.B:
                AppManager.Instance.dialogueScreen.Initialize(data.optionB, avatar);
                break;
            case Options.C:
                AppManager.Instance.dialogueScreen.Initialize(data.optionC, avatar);
                break;
            default:
                AppManager.Instance.dialogueScreen.Initialize(data.beforeText, avatar);
                break;
        }

        indicatorAnimation.AnimateOut();

        if (fadeSequence)
            fadeSequence.UpdateTalkedCount(character);
    }
}

[Serializable]
public enum InteractableCharacters
{
    NONE,
    drone_p_1,
    drone_p_2,
    drone_p_3,
    cat_p_1,
    cat_p_2,
    cat_p_3,
    pigeons_p_1,
    pigeons_p_2,
    pigeons_p_3
}
