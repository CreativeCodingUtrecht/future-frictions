using UnityEngine;

public class Outline : MonoBehaviour
{
    [SerializeField]
    private float outlineThickness;

    private Material outlineMaterial;

    private void Awake()
    {
        var rend = GetComponent<SpriteRenderer>();
        outlineMaterial = rend.material;

        SetOutlineThickness(outlineThickness);
    }

    public void SetOutlineEnabled(bool enabled)
    {
        if (!outlineMaterial)
        {
            var rend = GetComponent<SpriteRenderer>();
            outlineMaterial = rend.material;
        }

        outlineMaterial.SetFloat("_OutlineEnabled", enabled ? 1 : 0);
    }

    public void SetOutlineColor(Color color)
    {
        if (!outlineMaterial)
        {
            var rend = GetComponent<SpriteRenderer>();
            outlineMaterial = rend.material;
        }

        outlineMaterial.SetColor("_SolidOutline", color);
    }

    public void SetOutlineThickness(float thickness)
    {
        if (!outlineMaterial)
        {
            var rend = GetComponent<SpriteRenderer>();
            outlineMaterial = rend.material;
        }

        outlineMaterial.SetFloat("_Thickness", thickness);
    }
}