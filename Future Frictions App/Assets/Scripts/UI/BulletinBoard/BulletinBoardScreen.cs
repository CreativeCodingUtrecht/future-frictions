using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BulletinBoardScreen : BaseScreen
{
    [SerializeField]
    private Sprite errorImage;

    [SerializeField]
    private Color errorColor;

    [SerializeField]
    private BulletinBoard bulletinBoard;

    [SerializeField]
    private Button goButton;

    [SerializeField]
    private Button addButton;

    [SerializeField]
    private TMP_InputField commentInput;

    [SerializeField]
    private TextMeshProUGUI textDescription;

    [SerializeField]
    private BulletinBoardModal bulletinBoardModalPrefab;

    [SerializeField]
    private BulletinBoardModal userCreatedComment;

    [SerializeField]
    private Transform[] spawnPoints;

    private BulletinData bulletinData;
    private List<BulletinBoardModal> modals = new List<BulletinBoardModal>();

    private void Start()
    {
        AppManager.Instance.dataManager.SaveData();

        textDescription.text = DataManager.otherTextData["bulletin_text"];

        var comments = GetComments();
        CreateCommentModals(comments);

        goButton.onClick.AddListener(() =>
        {
            var data = transform.GetComponentsInChildren<BulletinBoardModal>();
            var commentsList = new List<BulletinComment>();

            foreach (var d in data)
            {
                if (d.bulletinComment.voteState != VoteState.NONE || d.bulletinComment.ID == -1)
                {
                    commentsList.Add(d.bulletinComment);
                }
            }

            bulletinBoard.ApplyChanges(commentsList.ToArray(), bulletinData);

            AppManager.Instance.uiManager.SetScreen(Screens.END);
        });

        addButton.onClick.AddListener(() =>
        {
            AddLocalComment(commentInput.text);
        });
    }

    private BulletinComment[] GetComments()
    {
        bulletinData = AppManager.Instance.dataManager.GetBulletinData();

        List<BulletinComment> comments = new List<BulletinComment>();

        foreach (var bD in bulletinData.data)
        {
            comments.Add(new BulletinComment() { ID = bD.id, Comment = bD.comment });
        }

        return comments.ToArray();
    }

    private void AddLocalComment(string comment)
    {
        addButton.interactable = false;

        if (string.IsNullOrEmpty(comment))
        {
            Debug.LogWarning("Empty");

            addButton.interactable = true;
            return;
        }

        // Check if not same as one we got
        foreach (var modal in modals)
        {
            if (modal.bulletinComment.Comment.ToLower() == comment.ToLower())
            {
                // Set error
                Debug.LogWarning("Already on screen");

                addButton.interactable = true;
                return;
            }
        }

        commentInput.interactable = false;

        userCreatedComment.gameObject.SetActive(true);
        userCreatedComment.Initialize(new BulletinComment() { ID = -1, Comment = comment });
    }

    public void AddComment(string comment)
    {
        addButton.interactable = false;

        // Check if less then 40 chars
        if (comment.Length > 40)
        {
            // Set error
            Debug.LogWarning("To long");

            //dialogUI.SetData("The comment is too long (keep it below 40 characters)", errorImage, errorColor);
            //dialogUI.Open();

            addButton.interactable = true;
            return;
        }

        // Check if not same as one we got
        foreach (var modal in modals)
        {
            if (modal.bulletinComment.Comment.ToLower() == comment.ToLower())
            {
                // Set error
                Debug.LogWarning("Already on screen");

                //dialogUI.SetData("This comment already exists", errorImage, errorColor);
                //dialogUI.Open();

                addButton.interactable = true;
                return;
            }
        }

        // Check with server if unique -> if not unique get data -> if unique add to bulletin board to push
        bulletinBoard.GetComment(comment, (id) =>
        {
            commentInput.interactable = false;

            userCreatedComment.gameObject.SetActive(true);
            userCreatedComment.Initialize(new BulletinComment() { ID = id, Comment = comment });
        }, () =>
        {
            addButton.interactable = true;
        });
    }

    public void CreateCommentModals(BulletinComment[] comments)
    {
        comments = comments.Reverse().ToArray();

        var pointCount = spawnPoints.Length;
        if (comments.Length < spawnPoints.Length)
        {
            pointCount = comments.Length;
        }

        for (int i = 0; i < pointCount; i++)
        {
            var newModal = Instantiate(bulletinBoardModalPrefab, spawnPoints[i]);
            newModal.Initialize(comments[i]);
            modals.Add(newModal);
        }
    }
}