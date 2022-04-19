using UnityEngine;
using DG.Tweening;

public enum AnimationType
{
    RiseFromGround,
    PopUp
}

public enum Axis
{
    X,
    Y,
    Z
}

public class WorldObject : MonoBehaviour
{
    [SerializeField]
    private AnimationType animationType;

    [SerializeField]
    private Axis axis;

    [SerializeField]
    private float animationTime = 3;

    [SerializeField]
    private float startValue;

    [SerializeField]
    private float endValue;

    public virtual void Awake()
    {
        SetStartState();
    }

    public virtual void AnimateIn()
    {
        Animate(true);
    }

    public virtual void AnimateOut()
    {
        Animate(false);
    }

    protected virtual void Animate(bool animateIn)
    {
        switch (animationType)
        {
            case AnimationType.PopUp:
                switch (axis)
                {
                    case Axis.Y:
                        transform.DOLocalRotate(new Vector3(transform.localRotation.eulerAngles.x, animateIn ? endValue : startValue, transform.localRotation.eulerAngles.z), animationTime);
                        break;

                    case Axis.Z:
                        transform.DOLocalRotate(new Vector3(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, animateIn ? endValue : startValue), animationTime);
                        break;

                    default:
                    case Axis.X:
                        transform.DOLocalRotate(new Vector3(animateIn ? endValue : startValue, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z), animationTime);
                        break;
                }
                break;

            default:
            case AnimationType.RiseFromGround:

                switch (axis)
                {
                    case Axis.X:
                        transform.DOMove(new Vector3(animateIn ? endValue : startValue, transform.position.y, transform.position.z), animationTime);
                        break;

                    case Axis.Y:
                        transform.DOMove(new Vector3(transform.position.x, animateIn ? endValue : startValue, transform.position.z), animationTime);
                        break;

                    case Axis.Z:
                        transform.DOMove(new Vector3(transform.position.x, transform.position.y, animateIn ? endValue : startValue), animationTime);
                        break;

                    default:
                        break;
                }

                break;
        }
    }

    public void SetStartState()
    {
        switch (animationType)
        {
            case AnimationType.RiseFromGround:
                transform.position = new Vector3(transform.position.x, startValue, transform.position.z);
                break;

            case AnimationType.PopUp:
                transform.rotation = Quaternion.Euler(startValue, transform.eulerAngles.y, transform.eulerAngles.z);
                break;

            default:
                break;
        }
    }
}