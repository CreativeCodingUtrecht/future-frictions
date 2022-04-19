using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogUI : BaseScreen
{
    [Header("Animation Vars")]
    [SerializeField]
    private RectTransform dialogueGroup;

    [SerializeField]
    private int startPosY;

    [SerializeField]
    private int endPosY;

    [SerializeField]
    private float animationTime;

    [Header("Refs")]
    [SerializeField]
    private Image avatar;

    [SerializeField]
    private TMP_Text textElement;

    [SerializeField]
    private float waitTime;

    private int animationID = -999;
    private bool isOpen;

    public override void Open()
    {
        if (animationID != -999)
        {
            DOTween.Pause(animationID);
        }

        base.Open();

        if (!isOpen)
        {
            dialogueGroup.DOMoveY(endPosY, animationTime).From(startPosY);
        }

        //StopAllCoroutines();
        //StartCoroutine(HideAfterTime());

        isOpen = true;
    }

    public void SetData(string text, Sprite avatarImage = null, Color color = default)
    {
        if (color == default)
        {
            color = Color.white;
        }

        avatar.sprite = avatarImage;
        avatar.color = color;

        textElement.text = text;
    }

    private IEnumerator HideAfterTime()
    {
        yield return new WaitForSeconds(waitTime);
        Close();
    }

    public override void Close()
    {
        StopAllCoroutines();

        var anim = dialogueGroup.DOMoveY(startPosY, animationTime).OnComplete(() => { gameObject.SetActive(false); });
        anim.SetId(1);
        animationID = anim.intId;

        isOpen = false;
    }
}