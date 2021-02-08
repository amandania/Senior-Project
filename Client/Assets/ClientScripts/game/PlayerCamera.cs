using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;
    public Transform lookPoint = null;


    public int mouseButton = 1; // right button by default
    
    public bool lockMovementToCam = false;
    public float xMouseSensitivity = 3f;
    public float yMouseSensitivity = 3f;
    public float yMinLimit = -40f;
    public float yMaxLimit = 80f;
    public float distance = 10;
    public float minDistance = 3;
    public float maxDistance = 10;

    public float zoomSpeedMouse = 1;
    public float zoomSpeedTouch = 0.2f;
    public float rotationSpeed = 5f;

    public float xMinAngle = -40;
    public float xMaxAngle = 80;
    private float targetFOV = 50;                                           // Target camera Field of View.


    // the target position can be adjusted by an offset in order to foucs on a
    // target's head for example
    public Vector3 offset = new Vector3(0.25f, 1.25f, 0);

    // view blocking
    // note: only works against objects with colliders.
    public LayerMask viewBlockingLayers;

    // store rotation so that unity never modifies it, otherwise unity will put
    // it back to 360 as soon as it's < 0, which makes a negative min angle impossible
    Vector3 rotation;


    void Awake()
    {
        rotation = transform.eulerAngles;
    }
    private void Update()
    {
        if (!target)
        {
            return;
        }

        if (Input.GetMouseButtonDown(1) && !lockMovementToCam)
        {
            Debug.Log("right click ");
            lockMovementToCam = true;
        }
        if (Input.GetMouseButtonUp(1) && lockMovementToCam)
        {
            lockMovementToCam = false;
        }
    }

    void LateUpdate()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (!target)
            return;

        // rotation and zoom should only happen if not in a UI right now

        Vector3 targetPos = target.position + offset;
        // right mouse rotation if we have a mouse
        rotation.y += Input.GetAxis("Mouse X") * rotationSpeed;
        rotation.x -= Input.GetAxis("Mouse Y") * rotationSpeed;
        rotation.x = Mathf.Clamp(rotation.x, xMinAngle, xMaxAngle);

        
       

        /*
         Quaternion camRotation = Quaternion.Euler(rotation.x, rotation.y, 0);
         transform.rotation = camRotation;
        // zoom
        float speed = Input.mousePresent ? zoomSpeedMouse : zoomSpeedTouch;
        float step = Utils.GetZoomUniversal() * speed;
        distance = Mathf.Clamp(distance - step, minDistance, maxDistance);


        // target follow
        transform.position = targetPos - (transform.rotation * Vector3.forward * distance);

        // avoid view blocking
        RaycastHit hit;
        if (Physics.Linecast(targetPos, transform.position, out hit, viewBlockingLayers))
        {
            // calculate a better distance (with some space between it)
            float d = Vector3.Distance(targetPos, hit.point) - 0.1f;

            // set the final cam position
            transform.position = targetPos - (transform.rotation * Vector3.forward * d);
        }*/
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
