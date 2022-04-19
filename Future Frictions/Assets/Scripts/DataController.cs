using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class DataController : MonoBehaviour
{
    [SerializeField]
    private ScreenshotController screenshotController;

    [SerializeField]
    private EnvironmentChanges environmentChanges;

    [Header("URL's")]
    [SerializeField]
    private string URL;

    [SerializeField]
    private string EditorURL = "http://localhost:3005/api";

    public static string URLBase;

    private void Awake()
    {
#if UNITY_EDITOR
        URLBase = EditorURL;
#else
        URLBase = string.IsNullOrEmpty(URL) ? Path.Combine(Application.absoluteURL, "api") : URL;
#endif

        Debug.Log($"The URL is {URLBase}");
    }

    public void CreateAndStoreData(string receiver, string subject, string message, Action<bool> callback)
    {
        // Take screenshot
        var screenshotBytes = screenshotController.TakeScreenshot();

        // Send data and receive ID
        StartCoroutine(
            StoreScreenshot(screenshotBytes, (ID) =>
            {
                if (!string.IsNullOrEmpty(ID))
                {
                    var scenarios = GetScenarioData();

                    var data = new SessionData()
                    {
                        SessionID = ID,

                        UserData = ApplicationManager.Instance.userData,

                        ResearcherEmail = "info@responsiblecities.nl",
                        PostcardReceiver = receiver,
                        PostcardSubject = subject,
                        PostcardMessage = message,

                        Scenarios = scenarios
                    };

                    var json = JsonUtility.ToJson(data, true);

                    StartCoroutine(StoreData(ID, json, callback));
                }
                else
                {
                    Debug.LogError("Got no ID");
                    callback?.Invoke(false);
                }
            })
        );
    }

    private SessionScenarioData[] GetScenarioData()
    {
        List<SessionScenarioData> selections = new List<SessionScenarioData>();

        foreach (var s in environmentChanges.scenarios)
        {
            var tmp = new SessionScenarioData()
            {
                scenarioID = s.ScenarioID.ToString(),
                selectedOption = s.SelectedOption.ToString()
            };

            selections.Add(tmp);
        }

        return selections.ToArray();
    }

    public IEnumerator StoreScreenshot(byte[] screenshotBytes, Action<string> callback)
    {
        WWWForm formData = new WWWForm();
        formData.AddBinaryData("imageData", screenshotBytes, "screenshot.png", "image/png");

        UnityWebRequest www = UnityWebRequest.Post($"{URLBase}/add-screenshot.php", formData);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            callback?.Invoke(null);
        }
        else
        {
            callback?.Invoke(www.downloadHandler.text);
        }
    }

    public IEnumerator StoreData(string ID, string json, Action<bool> callback)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("id", ID);
        formData.AddField("json", json);

        UnityWebRequest www = UnityWebRequest.Post($"{URLBase}/add-jsondata.php", formData);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            callback?.Invoke(false);
        }
        else
        {
            callback?.Invoke(true);
        }
    }

    public static IEnumerator SendMail(string receiver, string subject, string message, Action<bool> callback = null)
    {
        WWWForm formData = new WWWForm();
        formData.AddField("receiver", receiver);
        formData.AddField("subject", subject);
        formData.AddField("message", message);

        UnityWebRequest www = UnityWebRequest.Post("https://mail-backend.katpatat.com/mail", formData); // https://mail-backend.katpatat.com/mail
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            callback?.Invoke(false);
        }
        else
        {
            callback?.Invoke(true);
        }
    }
}

[Serializable]
internal class SessionData
{
    public string SessionID;

    public UserData UserData;

    public string ResearcherEmail;
    public string PostcardReceiver;
    public string PostcardSubject;
    public string PostcardMessage;

    public SessionScenarioData[] Scenarios;
}

[Serializable]
internal class SessionScenarioData
{
    public string scenarioID;
    public string selectedOption;
}