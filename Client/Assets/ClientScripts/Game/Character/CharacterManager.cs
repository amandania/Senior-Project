using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public GameObject DamagePefab;

    // Use this for initialization
    private void Start()
    {
        DamagePefab = Resources.Load("Prefabs/Damage") as GameObject;
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void TakeDamage(string a_damageText, float a_lifeTime = 2f)
    {
        var gameObj = Instantiate(DamagePefab, transform.position, Quaternion.identity, transform);
        var comp = gameObj.GetComponent<DamageLife>();
        comp.StartDamage(a_damageText, a_lifeTime);

    }
}

