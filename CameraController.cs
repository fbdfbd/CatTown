using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 10f;
    [SerializeField]
    private Vector2 movementRange = new Vector2(10f, 10f);
    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        HandleInput();
        MoveCamera();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButton(1))
        {
            float moveX = -Input.GetAxis("Mouse X") * speed * Time.deltaTime;
            targetPosition.x += moveX;
            targetPosition.x = Mathf.Clamp(targetPosition.x, -movementRange.x, movementRange.x);
        }
    }

    private void MoveCamera()
    {
        transform.position = new Vector3(targetPosition.x, transform.position.y, transform.position.z);
    }

    public void SetMovementRange(Vector2 newRange)
    {
        movementRange = newRange;
    }
}
