using UnityEngine;
/// <summary>
/// This class is used to lerp all server movement for any game object. Its not perfect but it kind of works. <see cref="HandleMoveCharacter"/>
/// We attach this to all monster or player game objects <see cref="HandleSpawnMonster"/> <seealso cref="HandleSpawnPlayer"/>
/// </summary>
public class MoveSync : MonoBehaviour
{
    //All movement is assigned we recieve any moveement packet
    public Vector3 endGoal;
    public Vector3 lastreal;
    public Quaternion lastrotation;
    public Quaternion endRotation;
    public float lerpTime;
    public float timeStartedLerping;
    public bool doLerp = false;
    public float Multipler = 15f;

    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        timeStartedLerping = Time.deltaTime;
    }

    /// <summary>
    /// This function is called after evey frame. When i did this in update it was a little jittery.
    /// </summary>
    private void FixedUpdate()
    {
        if (doLerp)
        {
            float lerpPercentage = (Time.deltaTime * Multipler - timeStartedLerping / lerpTime);

            transform.position = Vector3.Lerp(transform.position, endGoal, lerpPercentage);
            transform.transform.rotation = Quaternion.Lerp(transform.rotation, endRotation, lerpPercentage);
        }
    }

    //
    public void StartLerp()
    {
        timeStartedLerping = Time.deltaTime;
    }
}
