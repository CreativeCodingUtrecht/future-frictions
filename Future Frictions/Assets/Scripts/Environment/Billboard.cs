using UnityEngine;

public class Billboard : MonoBehaviour
{
    public enum BillboardType
    {
        Camera,
        LookAt
    }

    [SerializeField]
    private BillboardType billboardType;

    private Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        UpdateBillboard();
    }

    private void UpdateBillboard()
    {
        switch (billboardType)
        {
            case BillboardType.Camera:
                transform.rotation = Quaternion.Euler(transform.rotation.x, cameraTransform.rotation.eulerAngles.y, transform.rotation.z);
                break;

            case BillboardType.LookAt:
                transform.LookAt(new Vector3(cameraTransform.position.x, transform.position.y, cameraTransform.position.z));
                break;

            default:
                break;
        }
    }

    public void SetTarget(Transform target)
    {
        cameraTransform = target;
        UpdateBillboard();
    }
}