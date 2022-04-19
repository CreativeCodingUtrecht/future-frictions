using UnityEngine;

public class OldLady : MonoBehaviour
{
    [SerializeField]
    private Sprite alternativeSprite;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public void SwapSprite()
    {
        spriteRenderer.sprite = alternativeSprite;
    }
}