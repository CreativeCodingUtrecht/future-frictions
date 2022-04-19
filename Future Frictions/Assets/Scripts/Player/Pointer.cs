using System;
using UnityEngine;

public enum PointerState
{
    Pointer,
    Navigation
}

public class Pointer : MonoBehaviour
{
    [SerializeField]
    private PointerStateData pointerPointerData;

    [SerializeField]
    private PointerStateData navigationPointerData;

    [SerializeField]
    private Renderer meshRenderer;

    public void SetState(PointerState state)
    {
        switch (state)
        {
            case PointerState.Pointer:
                meshRenderer.material.mainTexture = pointerPointerData.Image;
                meshRenderer.material.color = pointerPointerData.Color;
                transform.localScale = pointerPointerData.Scale;
                break;

            case PointerState.Navigation:
                meshRenderer.material.mainTexture = navigationPointerData.Image;
                meshRenderer.material.color = navigationPointerData.Color;
                transform.localScale = navigationPointerData.Scale;
                break;

            default:
                break;
        }
    }
}

[Serializable]
public struct PointerStateData
{
    public PointerState State;
    public Texture2D Image;
    public Vector3 Scale;
    public Color Color;
}