using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player current;

    [Header("References")]
    public Camera playerCamera;

    [SerializeField]
    private Pointer pointer;

    [Header("Variables")]
    [SerializeField]
    private float moveSpeed = 8;

    [SerializeField]
    private float rotateSpeed = 10;

    private bool updatePointer = true;

    private Vector3 currentMoveTarget;
    private Vector2 rotation = Vector2.zero;

    private void Awake()
    {
        current = this;
        currentMoveTarget = transform.position;
    }

    private void Update()
    {
        if (!ApplicationManager.WorldInteractions)
        {
            return;
        }

        if (!HandleMovement())
        {
            HandleRotation();
        }

        MoveToPosition();
    }

    public void SetPointerState(bool state)
    {
        pointer.gameObject.SetActive(state);
        updatePointer = state;
    }

    public void SetCurrentMoveTarget(Vector3 newTarget)
    {
        currentMoveTarget = newTarget;
    }

    /// <summary>
    /// Returns true if valid target was clicked
    /// </summary>
    /// <returns></returns>
    private bool HandleMovement()
    {
        if (!updatePointer)
        {
            return false;
        }

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 15000))
        {
            pointer.gameObject.SetActive(true);
            pointer.transform.position = hit.point + hit.normal * 0.01f;

            if (hit.collider.CompareTag("PositionMarker") || hit.collider.CompareTag("Ground"))
            {
                pointer.SetState(PointerState.Navigation);
                pointer.transform.rotation = Quaternion.LookRotation(pointer.transform.position - transform.position) * Quaternion.Euler(90, 0, 0);

                if (Input.GetMouseButtonDown(0) && hit.collider.CompareTag("PositionMarker"))
                {
                    currentMoveTarget = new Vector3(hit.collider.transform.position.x, transform.position.y, hit.collider.transform.position.z);
                    return true;
                }
            }
            else
            {
                pointer.SetState(PointerState.Pointer);
                pointer.transform.LookAt(hit.point - hit.normal);
            }
        }
        else
        {
            pointer.gameObject.SetActive(false);
        }

        return false;
    }

    /// <summary>
    /// Rotates the camera and player based on the mouse input when the left mouse button is being pressed
    /// </summary>
    private void HandleRotation()
    {
        if (Input.GetMouseButton(0))
        {
            rotation.y += -Input.GetAxis("Mouse X");
            rotation.x += -Input.GetAxis("Mouse Y");
            rotation.x = Mathf.Clamp(rotation.x, -60f / rotateSpeed, 60f / rotateSpeed);
            transform.eulerAngles = new Vector2(0, rotation.y) * rotateSpeed;
            Camera.main.transform.localRotation = Quaternion.Euler(-rotation.x * rotateSpeed, 0, 0);
        }
    }

    /// <summary>
    /// Handles the movement towards the current target position
    /// </summary>
    private void MoveToPosition()
    {
        if (Vector3.Distance(transform.position, currentMoveTarget) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentMoveTarget, moveSpeed * Time.deltaTime);
        }
    }
}