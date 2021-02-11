using UnityEngine;
using System.Collections;

public class MoveSync : MonoBehaviour
{

    public Vector3 endGoal;
    public Vector3 lastreal;
    public Quaternion lastrotation;
    public Quaternion endRotation;
    public float lerpTime;
    public float timeStartedLerping;
    public bool doLerp = false;
    public float Multipler = 15f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timeStartedLerping = Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (doLerp) {
            float lerpPercentage = (Time.deltaTime * Multipler - timeStartedLerping / lerpTime);

            transform.position = Vector3.Lerp(transform.position, endGoal, lerpPercentage);
            transform.transform.rotation = Quaternion.Lerp(transform.rotation, endRotation, lerpPercentage);
            Debug.Log("lerping");
        }
    }

    public void StartLerp()
    {
        timeStartedLerping = Time.deltaTime;
    }
}
