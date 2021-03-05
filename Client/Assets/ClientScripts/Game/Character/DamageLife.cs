using UnityEngine;

public class DamageLife : MonoBehaviour
{
    public Transform GoalTransform { get; set; }
    public string DamageText;
    public Color color_i, color_f;
    public float LifeTime;
    public float StartTime;

    public bool Triggerd = false;
    public Vector3 startPos;
    public Transform bannerLookTarget;

    private void Awake()
    {
        enabled = false;
        //Debug.Log("turned off after instance");
        bannerLookTarget = Camera.main.transform;
    }

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

    private void LerpText(float progress)
    {
        transform.position = Vector3.Lerp(startPos, startPos + transform.up, progress);
    }
}
