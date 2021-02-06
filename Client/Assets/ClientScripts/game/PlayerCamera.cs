using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;
    public Transform lookPoint = null;


    public int mouseButton = 1; // right button by default

    public bool lockCamera;
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


    private float mouseY = 0f;
    private float mouseX = 0f;
    private float xMinLimit = -360f;
    private float xMaxLimit = 360f;

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
    }

    void LateUpdate()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (!target)
            return;

        // rotation and zoom should only happen if not in a UI right now

        // right mouse rotation if we have a mouse
        if (lookPoint != null)
        {
            Vector3 lookDir = (lookPoint.position) - target.position;
            Quaternion q = Quaternion.LookRotation(lookDir);
            Vector3 tempEuler = q.eulerAngles;
            q = Quaternion.Euler(new Vector3(Mathf.Clamp(tempEuler.x, 6, 25), tempEuler.y, tempEuler.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, Time.deltaTime * 85);
        }
        else
        {
            rotation.y += Input.GetAxis("Mouse X") * rotationSpeed;
            rotation.x -= Input.GetAxis("Mouse Y") * rotationSpeed;
            rotation.x = Mathf.Clamp(rotation.x, xMinAngle, xMaxAngle);
            transform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0);
            /*if (Input.mousePresent)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit2;
                    if (Physics.Raycast(ray, out hit2, 2))
                    {
                        Debug.Log(hit2.transform.name);

                    }
                }
            }
            else
            {
                // forced 45 degree if there is no mouse to rotate (for mobile)
                transform.rotation = Quaternion.Euler(new Vector3(45, 0, 0));
            }*/
        }

        // zoom
        float speed = Input.mousePresent ? zoomSpeedMouse : zoomSpeedTouch;
        float step = Utils.GetZoomUniversal() * speed;
        distance = Mathf.Clamp(distance - step, minDistance, maxDistance);


        // target follow
        transform.position = target.position - (transform.rotation * Vector3.forward * distance);

        // avoid view blocking
        RaycastHit hit;
        if (Physics.Linecast(target.position, transform.position, out hit, viewBlockingLayers))
        {
            // calculate a better distance (with some space between it)
            float d = Vector3.Distance(target.position, hit.point) - 0.1f;

            // set the final cam position
            transform.position = target.position - (transform.rotation * Vector3.forward * d);
        }
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
