using UnityEngine;

public class BaseScreen : MonoBehaviour
{
    public Screens Screen;

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}