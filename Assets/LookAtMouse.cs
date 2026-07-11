using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 mousePosition =
            mainCamera.ScreenToWorldPoint(Input.mousePosition);

        mousePosition.z = transform.position.z;

        Vector2 direction =
            mousePosition - transform.position;

        float angle =
            Mathf.Atan2(direction.y, direction.x) *
            Mathf.Rad2Deg;

        Quaternion targetRotation =
            Quaternion.Euler(0f, 0f, angle);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime);
    }
}