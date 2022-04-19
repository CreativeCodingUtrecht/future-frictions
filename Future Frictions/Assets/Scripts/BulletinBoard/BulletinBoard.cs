using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletinBoard : MonoBehaviour
{
    [SerializeField]
    private QuestManager questManager;

    //private const string urlBase = "https://ff-dev.katpatat.nl/api"; // "http://localhost:3005/api"

    public void GetComment(string comment, Action<int> onResult, Action onFailed = null)
    {
        StartCoroutine(FetchComment(comment, onResult, onFailed));
    }

    public void GetComments(Action<BulletinComment[]> onSucces, Action onFailed = null)
    {
        StartCoroutine(FetchComments(onSucces, onFailed));
    }

    public void AddComment(string comment, Action<int> onSucces, Action onFailed = null)
    {
        StartCoroutine(PostComment(comment, onSucces, onFailed));
    }

    public void ApplyChanges(BulletinComment[] bulletinComments)
    {
        foreach (var comment in bulletinComments)
        {
            if (comment.ID == -1)
            {
                // Add
                AddComment(comment.Comment, (id) =>
                {
                    if (comment.voteState == VoteState.UP)
                    {
                        // Upvote
                        StartCoroutine(UpvoteComment(id));
                    }
                    else if (comment.voteState == VoteState.DOWN)
                    {
                        // Downvote
                        StartCoroutine(DownvoteComment(id));
                    }
                });
            }
            else
            {
                if (comment.voteState == VoteState.UP)
                {
                    // Upvote
                    StartCoroutine(UpvoteComment(comment.ID));
                }
                else if (comment.voteState == VoteState.DOWN)
                {
                    // Downvote
                    StartCoroutine(DownvoteComment(comment.ID));
                }
            }
        }

        questManager.TriggerQuest(Quests.q_done);
    }

    private IEnumerator FetchComment(string comment, Action<int> onResult, Action onFailed)
    {
        WWWForm form = new WWWForm();
        form.AddField("comment", comment);

        using UnityWebRequest www = UnityWebRequest.Post($"{DataController.URLBase}/get-comment.php", form);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);

            onFailed?.Invoke();
        }
        else
        {
            string result = www.downloadHandler.text;

            if (int.TryParse(result, out int id))
            {
                onResult?.Invoke(id);
            }
            else if (result.Contains("no data"))
            {
                onResult?.Invoke(-1);
            }
            else
            {
                onFailed?.Invoke();
            }
        }
    }

    private IEnumerator FetchComments(Action<BulletinComment[]> onSucces, Action onFailed)
    {
        using UnityWebRequest www = UnityWebRequest.Get($"{DataController.URLBase}/get-comments.php");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);

            onFailed?.Invoke();
        }
        else
        {
            var result = www.downloadHandler.text;

            var data = result.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var comments = new List<BulletinComment>();

            for (var i = 0; i < data.Length; i++)
            {
                var dat = data[i].Split(',');
                var idString = dat[0].Trim();
            
                if (!int.TryParse(idString, out var id)) continue;
                
                var comment = new BulletinComment() {ID = id, Comment = dat[1]};
                comments.Add(comment);
            }

            onSucces?.Invoke(comments.ToArray());
        }
    }

    private IEnumerator PostComment(string comment, Action<int> onSucces, Action onFailed)
    {
        WWWForm form = new WWWForm();
        form.AddField("comment", comment);

        using UnityWebRequest www = UnityWebRequest.Post($"{DataController.URLBase}/add-comment.php", form);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);

            onFailed?.Invoke();
        }
        else
        {
            var result = www.downloadHandler.text;

            if (int.TryParse(result, out int id))
            {
                onSucces?.Invoke(id);
            }
        }
    }

    public IEnumerator UpvoteComment(int id)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);

        using UnityWebRequest www = UnityWebRequest.Post($"{DataController.URLBase}/upvote-comment.php", form);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        //else
        //{
        //    var res = www.downloadHandler.text;

        //    Debug.Log($"Upvoted id = {id} | Message: {res}");
        //}
    }

    public IEnumerator DownvoteComment(int id)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);

        using UnityWebRequest www = UnityWebRequest.Post($"{DataController.URLBase}/downvote-comment.php", form);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        //else
        //{
        //    var res = www.downloadHandler.text;

        //    Debug.Log($"Downvoted id = {id} | Message: {res}");
        //}
    }
}

public class BulletinComment
{
    public int ID;
    public string Comment;
    public VoteState voteState = VoteState.NONE;
}