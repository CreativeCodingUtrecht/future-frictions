using DG.Tweening;
using UnityEngine;

public class BulletinBankje : MonoBehaviour
{
    [SerializeField]
    private BulletinBoardScreen bulletinBoardScreen;

    [SerializeField]
    private Color outlineColor;

    [SerializeField]
    private Outline outline;

    [SerializeField]
    private Player player;

    [SerializeField]
    private AnimateUpAndDown arrowAnimation;

    private bool isActive;
    private bool doDistanceCheck;

    private void Start()
    {
        outline.SetOutlineEnabled(isActive);
        outline.SetOutlineColor(outlineColor);

        arrowAnimation.gameObject.SetActive(isActive);
    }

    public void ActivateBulletinBankje()
    {
        isActive = true;

        arrowAnimation.gameObject.SetActive(true);
        arrowAnimation.RunAnimation();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && isActive)
        {
            StartSequence();
        }
    }

    private void OnMouseEnter()
    {
        outline.SetOutlineEnabled(isActive);
    }

    private void OnMouseExit()
    {
        outline.SetOutlineEnabled(false);
    }

    private void StartSequence()
    {
        Debug.Log("Start bankie sequence");

        player.SetCurrentMoveTarget(new Vector3(transform.position.x, player.transform.position.y, transform.position.z));
        doDistanceCheck = true;
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.B))
        {
            bulletinBoardScreen.Open();
        }
        
        // Check distance to player
        if (doDistanceCheck && Vector3.Distance(transform.position, player.transform.position) < 0.1f)
        {
            doDistanceCheck = false;

            ApplicationManager.SetWorldInteractionsState(false);

            // Rotate camera to sky
            var animationTime = 4f;
            var cameraTransform = player.playerCamera.transform;
            cameraTransform.DOLocalRotate(new Vector3(-37, 0, 0), animationTime).OnComplete(() =>
            {
                bulletinBoardScreen.Open();
            });
        }
    }
}