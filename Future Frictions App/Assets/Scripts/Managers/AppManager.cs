using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance;

    public DataManager dataManager;
    public UIManager uiManager;

    public DialogueScreen dialogueScreen;

    private void Awake()
    {
        Instance = this;

        var path = Path.Combine(Application.persistentDataPath, "ffbulletin.json");
        if (!File.Exists(path))
        {
            Debug.Log("New file");

            var file = Resources.Load<TextAsset>("Bulletin/ffbulletin");

            using (var sW = new StreamWriter(path))
            {
                sW.Write(file.text);
            }
        }
    }

    private void Start()
    {
        uiManager.SetScreen(Screens.INTRO);
        dataManager.GetTextData();
    }
    
    public void RestartApp()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
