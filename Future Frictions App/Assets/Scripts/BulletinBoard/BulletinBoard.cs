using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class BulletinBoard : MonoBehaviour
{
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

    public void ApplyChanges(BulletinComment[] bulletinComments, BulletinData bulletinData)
    {
        foreach (var comment in bulletinComments)
        {
            if (comment.ID == -1)
            {
                bulletinData.data.Add(new BulletinEntry() { id = GetID(bulletinData), comment = comment.Comment });
            }
            else
            {
                BulletinEntry entry = null;

                switch (comment.voteState)
                {
                    case VoteState.UP:
                    case VoteState.DOWN:
                        entry = bulletinData.data.FirstOrDefault(x => x.id == comment.ID);
                        if (entry != null)
                        {
                            switch (comment.voteState)
                            {
                                case VoteState.UP:
                                    entry.upvotes++;
                                    break;
                                case VoteState.DOWN:
                                    entry.downvotes++;
                                    break;

                            }
                        }
                        break;
                }
            }
        }

        // Save data to disk
        var json = JsonUtility.ToJson(bulletinData);

        using (StreamWriter sW = new StreamWriter(Path.Combine(Application.persistentDataPath, "ffbulletin.json"), false))
        {
            sW.Write(json);
        }

        Debug.Log("Written to file");
    }

    private int GetID(BulletinData bulletinData)
    {
        int id = -1;

        foreach (var bD in bulletinData.data)
        {
            if (bD.id > id)
            {
                id = bD.id;
            }
        }

        return id + 1;
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
            string result = www.downloadHandler.text;

            var data = result.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var comments = new BulletinComment[data.Length];

            for (int i = 0; i < comments.Length; i++)
            {
                var dat = data[i].Split(',');

                var comment = new BulletinComment() { ID = int.Parse(dat[0]), Comment = dat[1] };
                comments[i] = comment;
            }

            onSucces?.Invoke(comments);
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
    }
}

public class BulletinComment
{
    public int ID;
    public string Comment;
    public VoteState voteState = VoteState.NONE;
}