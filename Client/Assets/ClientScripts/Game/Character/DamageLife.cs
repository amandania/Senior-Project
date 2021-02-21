using UnityEngine;

public class DamageLife : MonoBehaviour
{
    public Transform GoalTransform { get; set; }
    public string DamageText;
    public Color color_i, color_f;
    public float LifeTime;
    public float StartTime;

    private bool Triggerd { get; set; } = false;

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
        Triggerd = true;
        enabled = true;
    }

    public void Update()
    {
        if (Triggerd)
        {
            float progress = (Time.time - StartTime) / LifeTime;
            if (progress <= 1)
            {
                transform.localPosition = Vector3.Lerp(transform.position, transform.Find("GoalPos").transform.position, progress);
            }
            else
            {
                Destroy(transform.gameObject);
            }

        }

    }

}
