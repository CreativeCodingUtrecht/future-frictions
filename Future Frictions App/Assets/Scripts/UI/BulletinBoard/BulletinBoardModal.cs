using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BulletinBoardModal : MonoBehaviour
{
    [SerializeField]
    private Color LikeColor;

    [SerializeField]
    private Color DislikeColor;

    [SerializeField]
    private Color NeutralColor;

    [SerializeField]
    private TextMeshProUGUI commentText;

    [SerializeField]
    private Button likeButton;

    [SerializeField]
    private Button dislikeButton;

    public BulletinComment bulletinComment;

    public void Initialize(BulletinComment bulletinComment)
    {
        this.bulletinComment = bulletinComment;

        commentText.text = bulletinComment.Comment;

        likeButton.onClick.AddListener(() =>
        {
            if (bulletinComment.voteState == VoteState.UP)
            {
                bulletinComment.voteState = VoteState.NONE;
            }
            else
            {
                bulletinComment.voteState = VoteState.UP;
            }

            SetVisualState();
        });

        dislikeButton.onClick.AddListener(() =>
        {
            if (bulletinComment.voteState == VoteState.DOWN)
            {
                bulletinComment.voteState = VoteState.NONE;
            }
            else
            {
                bulletinComment.voteState = VoteState.DOWN;
            }

            SetVisualState();
        });
    }

    private void SetVisualState()
    {
        switch (bulletinComment.voteState)
        {
            case VoteState.NONE:
                likeButton.image.color = NeutralColor;
                dislikeButton.image.color = NeutralColor;
                break;

            case VoteState.UP:
                likeButton.image.color = LikeColor;
                dislikeButton.image.color = NeutralColor;
                break;

            case VoteState.DOWN:
                likeButton.image.color = NeutralColor;
                dislikeButton.image.color = DislikeColor;
                break;

            default:
                break;
        }
    }
}

public enum VoteState
{
    NONE,
    UP,
    DOWN
}