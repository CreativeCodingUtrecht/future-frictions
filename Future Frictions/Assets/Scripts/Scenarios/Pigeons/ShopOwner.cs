using UnityEngine;
using UnityEngine.UI;

public class ShopOwner : MonoBehaviour
{
    [SerializeField]
    private Sprite alternativeSprite;

    [SerializeField]
    private Vector3 alternativeScale;

    [SerializeField]
    private Vector3 alternativePosition;
    
    public void SwapImage()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = alternativeSprite;
        
        var trans = transform;
        
        trans.localScale = alternativeScale;
        trans.localPosition = alternativePosition;
    }
}
