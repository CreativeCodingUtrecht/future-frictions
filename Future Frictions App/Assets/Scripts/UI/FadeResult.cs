using UnityEngine;

public class FadeResult : FadePanel
{
    [SerializeField]
    private float fadeSpeed;

    private void Start()
    {
        FadeIn(fadeSpeed);
    }
}
