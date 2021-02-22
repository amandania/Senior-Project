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

    private void Awake()
    {
        enabled = false;
        Debug.Log("turned off after instance");
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
            float progress = (Time.time - StartTime) / LifeTime;
            //print("progress : " + progress);
            if (progress <= 1)
            {
                print("lerping");
                transform.position = Vector3.Lerp(startPos, startPos + (Vector3.back + Vector3.up) * 2, progress);
            }
            else
            {
                Destroy(transform.gameObject);
            }

        }

    }

}
