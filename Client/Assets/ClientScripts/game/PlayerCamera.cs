using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform followPart;
    public Transform lookPoint = null;


    public int mouseButton = 1; // right button by default

    public bool lockMovementToCam = false;
    public float xMouseSensitivity = 1f;
    public float yMouseSensitivity = 1f;
    public float yMinLimit = 20f;
    public float yMaxLimit = 70f;
    public float distance = 3;
    public float minDistance = 2;
    public float maxDistance = 4;

    public float zoomSpeedMouse = 1;
    public float zoomSpeedTouch = 0.2f;
    public float rotationSpeed = 5f;

    public float xMinAngle = -40;
    public float xMaxAngle = 80;                                        // Target camera Field of View.


    // the target position can be adjusted by an offset in order to foucs on a
    // target's head for example
    public Vector3 offset = new Vector3(0f, 0f, 0f);
    public Transform playerTarget;

    // view blocking
    // note: only works against objects with colliders.
    public LayerMask viewBlockingLayers;

    // store rotation so that unity never modifies it, otherwise unity will put
    // it back to 360 as soon as it's < 0, which makes a negative min angle impossible
    private Vector3 rotation;

    private void Awake()
    {
        rotation = transform.eulerAngles;

    }
    private void Update()
    {
        if (!followPart)
        {
            return;
        }

        if (Input.GetMouseButtonDown(1) && !lockMovementToCam)
        {
            //Debug.Log("right click ");
            lockMovementToCam = true;
        }
        if (Input.GetMouseButtonUp(1) && lockMovementToCam)
        {
            lockMovementToCam = false;
        }
    }

    private void LateUpdate()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (!followPart)
            return;

        // rotation and zoom should only happen if not in a UI right now

        Vector3 targetPos = followPart.position + offset;
        // right mouse rotation if we have a mouse
        rotation.y += Input.GetAxis("Mouse X") * rotationSpeed;
        rotation.x -= Input.GetAxis("Mouse Y") * rotationSpeed;
        rotation.x = Mathf.Clamp(rotation.x, xMinAngle, xMaxAngle);






        Quaternion camRotation = Quaternion.Euler(rotation.x, rotation.y, 0);
        followPart.transform.rotation = camRotation;
        //transform.rotation = camRotation;


        // zoom
        float speed = Input.mousePresent ? zoomSpeedMouse : zoomSpeedTouch;
        float step = Utils.GetZoomUniversal() * speed;
        distance = Mathf.Clamp(distance - step, minDistance, maxDistance);


        // target follow
        //transform.position = targetPos - (transform.rotation * Vector3.forward * distance);
        transform.position = targetPos - (playerTarget.rotation * Vector3.forward * distance);


    }

    public float ClampAngle(float angle, float min, float max)
    {
        do
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
        } while (angle < -360 || angle > 360);

        return Mathf.Clamp(angle, min, max);
    }
}
