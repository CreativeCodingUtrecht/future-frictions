using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class is responsible for managaging the different data needed
/// </summary>
public class DataManager : MonoBehaviour
{
    public static event Action AppTextAvailable;
    public static event Action AppScenarioTextAvailable;

    public static event Action LanguageChanged;

    public static UserData CurrentUserData { get; private set; }
    public ScenarioData CurrentScenario { get; private set; } = new ScenarioData() { Scenario = Scenarios.NONE };

    public static string CurrentLanguage { get; private set; } = "en-EN";

    public static Dictionary<string, string> otherTextData;
    public static Dictionary<string, ScenarioTextData> scenariosTextData;

    public ScenarioData[] scenarios;

    private Downloader downloader;

    private Dictionary<string, Dictionary<string, string>> textDataLanguageDictionary;
    private Dictionary<string, Dictionary<string, ScenarioTextData>> scenarioDataLanguageDictionary;

    private void Awake()
    {
        downloader = gameObject.AddComponent<Downloader>();

        CurrentLanguage = "en-EN";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ChangeCurrentLanguage(CurrentLanguage == "en-EN" ? "nl-NL" : "en-EN");
        }
    }

    public static void SetUserData(UserData userData)
    {
        CurrentUserData = userData;
    }

    public ScenarioData GetQuestionData()
    {
        ScenarioData scenario = null;

        switch (CurrentScenario.Scenario)
        {
            case Scenarios.NONE:
                scenario = scenarios.FirstOrDefault(x => x.Scenario == Scenarios.drone_s_1);
                break;
            case Scenarios.drone_s_1:
                scenario = scenarios.FirstOrDefault(x => x.Scenario == Scenarios.cat_s_1);
                break;
            case Scenarios.cat_s_1:
                scenario = scenarios.FirstOrDefault(x => x.Scenario == Scenarios.pigeons_s_1);
                break;
            case Scenarios.pigeons_s_1:
            default:
                break;
        }

        CurrentScenario = scenario;
        return scenario;
    }

    public void GetTextData()
    {
        downloader.StartDownload(new string[] { "OtherText", "ScenariosText" }, (stringData) =>
        {
            var csvParser = new CSVParser();
            
            textDataLanguageDictionary = csvParser.GetTextDataLanguageDictionary(stringData[0]);
            scenarioDataLanguageDictionary = csvParser.GetScenarioLanguageDictionary(stringData[1]);

            otherTextData = textDataLanguageDictionary[CurrentLanguage];
            scenariosTextData = scenarioDataLanguageDictionary[CurrentLanguage];
            
            AppTextAvailable?.Invoke();
            AppScenarioTextAvailable?.Invoke();
        });
    }

    public void ChangeCurrentLanguage(string lang)
    {
        CurrentLanguage = lang;
        
        otherTextData = textDataLanguageDictionary[CurrentLanguage];
        scenariosTextData = scenarioDataLanguageDictionary[CurrentLanguage];
        
        Debug.Log($"Changed language to {lang}");
        
        LanguageChanged?.Invoke();
    }

    public BulletinData GetBulletinData()
    {
        var path = Path.Combine(Application.persistentDataPath, "ffbulletin.json");
        var json = File.ReadAllText(path);

        var data = JsonUtility.FromJson<BulletinData>(json);

        return data;
    }

    public void SaveData()
    {
        var s = new List<SD>();

        foreach (var sc in scenarios)
        {
            s.Add(new SD()
            {
                Scenario = sc.Scenario,
                SelectedOption = sc.SelectedOption
            });
        };

        var saveData = new SaveData()
        {
            userData = CurrentUserData,
            scenarioData = s.ToArray()
        };

        var json = JsonUtility.ToJson(saveData, true);
        var dirPath = Path.Combine(Application.persistentDataPath, "Data");

        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        var path = Path.Combine(dirPath, $"{Guid.NewGuid()}.json");

        using (var sW = new StreamWriter(path))
        {
            sW.Write(json);
        }
    }
}

[Serializable]
public class UserData
{
    public string Name;
    public string Age;
    public string Email;
    public string Location;
}

[Serializable]
public class ScenarioData
{
    public Scenarios Scenario;
    public Options SelectedOption;

    public Sprite[] avatarImages;
}

[Serializable]
public enum Scenarios
{
    NONE,
    drone_s_1,
    cat_s_1,
    pigeons_s_1
}

[Serializable]
public enum Options
{
    NONE,
    A,
    B,
    C
}

[Serializable]
public class BulletinData
{
    public List<BulletinEntry> data;
}

[Serializable]
public class BulletinEntry
{
    public int id;
    public string comment;
    public int upvotes;
    public int downvotes;
}

[Serializable]
public class SaveData
{
    public UserData userData;
    public SD[] scenarioData;
}

[Serializable]
public class SD
{
    public Scenarios Scenario;
    public Options SelectedOption;
}