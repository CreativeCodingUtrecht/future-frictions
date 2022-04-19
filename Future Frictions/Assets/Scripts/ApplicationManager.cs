using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ApplicationManager : MonoBehaviour
{
    public static event Action ScenarioTextDataAvailable;

    public static event Action StartScreenTextDataAvailable;
    
    public static event Action LanguageChanged;

    public static Dictionary<string, ScenarioTextData> scenariosTextData;
    public static Dictionary<string, string> questTextData;
    public static Dictionary<string, string> startScreenData;

    public static ApplicationManager Instance;

    public static event Action<bool> OnInteractableChanged;

    public static bool WorldInteractions { get; private set; }
    
    public static string CurrentLanguage { get; private set; } = "en-EN";

    public InteractionStateData[] stateData;

    public UserData userData;

    [SerializeField] private CSVDownloader csvDownloader;

    [SerializeField]
    private bool resetAfterTime = false;

    [SerializeField]
    private float resetTime = 300;

    private float timer = 0;

    private Dictionary<string, Dictionary<string, ScenarioTextData>> scenarioDataLanguageDictionary;
    private Dictionary<string, Dictionary<string, string>> startScreenDataLanguageDictionary;
    private Dictionary<string, Dictionary<string, string>> questDataLanguageDictionary;
    
    public static void SetWorldInteractionsState(bool interactable)
    {
        WorldInteractions = interactable;

        OnInteractableChanged?.Invoke(WorldInteractions);
    }

    private void Awake()
    {
        Instance = this;
        CurrentLanguage = "en-EN";
    }

    private void Start()
    {
        csvDownloader.StartDownload((stringData) =>
        {
            var csvParser = new CSVParser();

            // Start Screen
            startScreenDataLanguageDictionary = csvParser.GetStartScreenLanguageDictionary(stringData[1]);

            // Scenarios 
            scenarioDataLanguageDictionary = csvParser.GetScenarioLanguageDictionary(stringData[0]);

            // Quests
            questDataLanguageDictionary = csvParser.GetQuestLanguageDictionary(stringData[3]);

            startScreenData = startScreenDataLanguageDictionary[CurrentLanguage];
            scenariosTextData = scenarioDataLanguageDictionary[CurrentLanguage];
            questTextData = questDataLanguageDictionary[CurrentLanguage];

            StartScreenTextDataAvailable?.Invoke();
            ScenarioTextDataAvailable?.Invoke();
        });
    }

    private void Update()
    {
        if (resetAfterTime)
        {
            if (Input.anyKeyDown)
            {
                timer = 0;
            }

            timer += Time.deltaTime;

            if (timer >= resetTime)
            {
                resetAfterTime = false;

                // Reset
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }
    }

    public void ResetScenarios()
    {
        var drone = FindObjectOfType<Drone>();
        drone.ResetDrone();

        var questManager = FindObjectOfType<QuestManager>();
        questManager.ResetQuests();

        var environmentChanges = FindObjectOfType<EnvironmentChanges>();
        environmentChanges.ResetData();
    }
    
    public void ChangeCurrentLanguage(string lang)
    {
        CurrentLanguage = lang;
        
        startScreenData = startScreenDataLanguageDictionary[CurrentLanguage];
        questTextData = questDataLanguageDictionary[CurrentLanguage];
        scenariosTextData = scenarioDataLanguageDictionary[CurrentLanguage];
        
        Debug.Log($"Changed language to {lang}");
        
        LanguageChanged?.Invoke();
    }
}

[Serializable]
public class InteractionStateData
{
    public InteractionState State;
    public Sprite Sprite;
    public string Text;
    public Color Color;
    public Material material;
}

[Serializable]
public enum InteractionState
{
    NONE,
    New,
    Updated,
    Active,
    Read,
    ChoiceA,
    ChoiceB,
    ChoiceC
}

[Serializable]
public class UserData
{
    public string Name;
    public string Age;
    public string Email;
    public string Location;
}