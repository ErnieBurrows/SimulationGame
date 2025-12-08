using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Rotation")]
    public float rotationSpeed = 5f;
    public float minPitch = -80f;
    public float maxPitch = 80f;

    [Header("Zoom")]
    public float zoomSpeed = 5f;
    public float minZoom = 2f;
    public float maxZoom = 40f;

    [Header("Panning")]
    public float panSpeed = 0.5f;

    private Camera cam;
    private float yaw;      // rotation around Y
    private float pitch;    // rotation around X

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;

        // Initialize pitch/yaw from current camera rotation
        Vector3 e = transform.eulerAngles;
        pitch = e.x;
        yaw = e.y;
    }

    void Update()
    {
        HandleRotation();
        HandleZoom();
        HandlePan();
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(1))
        {
            float mx = Input.GetAxis("Mouse X");
            float my = Input.GetAxis("Mouse Y");

            yaw += mx * rotationSpeed;
            pitch -= my * rotationSpeed;

            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);  // <-- rotate on X & Y ONLY
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }

    void HandlePan()
    {
        if (Input.GetMouseButton(2))
        {
            float mx = Input.GetAxis("Mouse X");
            float my = Input.GetAxis("Mouse Y");

            // Pan relative to camera orientation
            Vector3 right = transform.right;
            Vector3 up = transform.up;

            // Scale pan speed by zoom so it feels consistent
            float scale = cam.orthographicSize * 0.02f;

            Vector3 move = (-right * mx + -up * my) * panSpeed * scale;

            transform.position += move;
        }
    }
}
