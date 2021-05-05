using UnityEngine;

/// <summary>
/// This class is a monobeahvior and is a attached to a prefab. 
/// We parent the prefab to the workspace area to trigger the start beavior. 
/// We use this this class to create the visual representation for incoming damage and the lifetime it should stay for.
/// This prefab containg this class gets destroy after the lifetime is reached resulting in unity cleaning itself up.
/// </summary>

public class DamageLife : MonoBehaviour
{
    //The goal object in prefab to get offset to travel
    public Transform GoalTransform { get; set; }


    //Dynamic text changed during Instance create
    public string DamageText;

    //Default text Colors for fade
    public Color color_i, color_f;

    //Dynamic Lifetime information
    public float LifeTime;
    public float StartTime;

    //Clas startup information
    public bool Triggerd = false;
    public Vector3 startPos;

    //Camera realtive transformation details for good cam popups for the text
    public Transform bannerLookTarget;


    /// <summary>
    /// This function is triggered before we start listening for updates 
    /// We disable the class since we havent trigered it yet but we set our current camera tranformation
    /// </summary>
    private void Awake()
    {
        enabled = false;
        //Debug.Log("turned off after instance");
        bannerLookTarget = Camera.main.transform;
    }

    /// <summary>
    /// Main function triggered by our Damage packet handler <see cref="HandleDamageMessage"/>
    /// We set all our required data for the Update() to handle accordingly.
    /// </summary>
    /// <param name="a_damage">The text to show</param>
    /// <param name="a_lifeTime">How long does this text show for. Defaults is 1 second</param>
    public void StartDamage(string a_damage, float a_lifeTime = 1f)
    {
        DamageText = a_damage;
        LifeTime = a_lifeTime;
        GetComponent<TextMesh>().text = a_damage;
        StartTime = Time.time;
        startPos = transform.position;
        Triggerd = true;
        enabled = true;
    }

    /// <summary>
    /// Call every frame by unity engine
    /// This function checks what % of the life time has covered and if its less then 100% or 1 in this case we are moving towards our goalpos 
    /// </summary>
    public void Update()
    {
        if (Triggerd)
        {
            //percentage of life traveled 
            float progress = (Time.time - StartTime) / LifeTime;
            ////print("progress : " + progress);
            if (progress <= 1)
            {
                //Look vector too keep our damage rotation towards our camera view 
                Vector3 lookDir = transform.position - (bannerLookTarget.position);

                LerpText(progress);

                transform.rotation = Quaternion.LookRotation(lookDir);
            }
            else
            {
                Destroy(transform.gameObject);
            }
            
        }

    }

    //Function call to have the text move smoothly each frame to its goal pos
    private void LerpText(float progress)
    {
        transform.position = Vector3.Lerp(startPos, startPos + transform.up, progress);
    }
}
