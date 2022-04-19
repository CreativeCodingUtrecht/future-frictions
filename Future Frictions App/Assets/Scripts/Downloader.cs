using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Downloader : MonoBehaviour
{
    private bool completed;

    public void StartDownload(string[] fileNames, Action<string[]> onCompleted)
    {
        // @neander: TODO: Check internet -> download updated data and store else check if is in data path else load from resources
        
        var downloadData = new string[fileNames.Length];

        for (int i = 0; i < fileNames.Length; i++)
        {
            var textData = Resources.Load<TextAsset>(fileNames[i]);
            if (textData != null)
            {
                downloadData[i] = textData.text;
                completed = true;
            }
        }

        if (completed)
        {
            onCompleted(downloadData);
        }
    }

    public void StartDownload(string googleSheetID, string[] tabIDs, Action<string[]> onCompleted)
    {
        StartCoroutine(DownloadData(googleSheetID, tabIDs, onCompleted));
    }

    private IEnumerator DownloadData(string googleSheetID, string[] tabIDs, Action<string[]> onCompleted)
    {
        var downloadData = new string[tabIDs.Length];

        for (var i = 0; i < tabIDs.Length; i++)
        {
            using var webRequest =
                UnityWebRequest.Get(
                    $"https://docs.google.com/spreadsheets/d/{googleSheetID}/export?format=csv&gid={tabIDs[i]}");
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + webRequest.error);
                    downloadData[i] = PlayerPrefs.GetString($"LastDataDownloaded{i}");
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + webRequest.error);
                    downloadData[i] = PlayerPrefs.GetString($"LastDataDownloaded{i}");
                    break;
                case UnityWebRequest.Result.Success:
                    // Debug.Log("Received: " + webRequest.downloadHandler.text);
                    PlayerPrefs.SetString($"LastDataDownloaded{i}", webRequest.downloadHandler.text);
                    downloadData[i] = webRequest.downloadHandler.text;
                    completed = true;
                    break;
                case UnityWebRequest.Result.InProgress:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        if (completed)
        {
            onCompleted(downloadData);
        }
    }
}