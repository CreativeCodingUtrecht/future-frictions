using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class CSVDownloader : MonoBehaviour
{
  private  const string GoogleSheetID = "1j5yfsLR1PbUl_lyM8pZDToLxX2vdLyR7t3fr8gg-yBI";

  private bool completed;
  
  /// <summary>
  /// [0] = scenarios
  /// [1] = start screen
  /// [2] = postcard
  /// [3] = quests
  /// </summary>
  private readonly string[] tabIDs = { "1735467200", "1639534776", "858662302", "2082194516" };

  public void StartDownload(Action<string[]> onCompleted)
  {
    StartCoroutine(DownloadData(onCompleted));
  }

  private IEnumerator DownloadData(Action<string[]> onCompleted)
  {
    var downloadData = new string[tabIDs.Length];

    for (var i = 0; i < tabIDs.Length; i++)
    {
      using var webRequest =
        UnityWebRequest.Get(
          $"https://docs.google.com/spreadsheets/d/{GoogleSheetID}/export?format=csv&gid={tabIDs[i]}");
      yield return webRequest.SendWebRequest();

      switch (webRequest.result)
      {
        case UnityWebRequest.Result.ConnectionError:
        case UnityWebRequest.Result.DataProcessingError:
          Debug.LogError("Error: " + webRequest.error);
          downloadData[i] = PlayerPrefs.GetString($"LastDataDownloaded{i}");
          break;
        case UnityWebRequest.Result.ProtocolError:
          Debug.LogError( "HTTP Error: " + webRequest.error);
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