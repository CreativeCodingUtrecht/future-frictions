using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class handles the different states the UI can be in
/// </summary>
public class UIManager : MonoBehaviour
{
    private BaseScreen[] screens;

    private void Awake()
    {
        screens = FindObjectsOfType<BaseScreen>(true);
    }

    public void SetScreen(Screens screen)
    {
        AppManager.Instance.dialogueScreen.Hide();

        foreach (var s in screens)
        {
            if (s.Screen != screen)
            {
                s.Close();
            }
            else
            {
                s.Open();
            }
        }
    }

    public T GetScreen<T>() where T : BaseScreen
    {
        return (T)screens.FirstOrDefault(x => x is T);
    }
}

public enum Screens
{
    NONE,
    INTRO,
    QUESTION,
    RESULT,
    BULLETIN,
    END,
    DIALOGUE
}