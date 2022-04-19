using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusCube : MonoBehaviour
{
    [SerializeField]
    private Image[] icons;

    [SerializeField]
    private TextMeshProUGUI[] texts;

    [SerializeField]
    private PulseScale pulseScaleAnimation;

    public void SetStatus(InteractionStateData stateData)
    {
        if (stateData.State != InteractionState.NONE)
        {
            var rend = GetComponentInChildren<Renderer>();
            rend.material = stateData.material;
            gameObject.SetActive(true);
        }

        // Set data
        switch (stateData.State)
        {
            case InteractionState.New:
            case InteractionState.Updated:
            case InteractionState.Active:
                pulseScaleAnimation.SetSelectedState(false, null);

                foreach (var t in texts)
                {
                    t.gameObject.SetActive(false);
                }

                foreach (var i in icons)
                {
                    i.gameObject.SetActive(true);
                    i.sprite = stateData.Sprite;
                }
                break;

            case InteractionState.Read:
                pulseScaleAnimation.SetSelectedState(true, null);

                foreach (var t in texts)
                {
                    t.gameObject.SetActive(false);
                }

                foreach (var i in icons)
                {
                    i.gameObject.SetActive(true);
                    i.sprite = stateData.Sprite;
                }
                break;

            case InteractionState.ChoiceA:
            case InteractionState.ChoiceB:
            case InteractionState.ChoiceC:
                foreach (var t in texts)
                {
                    t.gameObject.SetActive(true);
                }

                foreach (var i in icons)
                {
                    i.gameObject.SetActive(false);
                }
                break;

            default:
            case InteractionState.NONE:
                gameObject.SetActive(false);
                break;
        }
    }
}