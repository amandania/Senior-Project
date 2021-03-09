using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class is used to control active weapons and armors equipped to parent gameobject.
/// We trigger the functions through invoke callls from the HandleEquipment Packet
/// </summary>

public class Equipment : MonoBehaviour
{
    //Map of transform name and current equipmodel
    public Dictionary<Transform, GameObject> ItemToTransformMap;


    //Map of loadedTransform names;
    public Dictionary<string, Transform> TransformMap;

    // Use this for initialization
    private void Start()
    {
        TransformMap = new Dictionary<string, Transform>();
        ItemToTransformMap = new Dictionary<Transform, GameObject>();
    }

    // Update is called once per frame
    private void Update()
    {

    }
    /// <summary>
    /// This function is invoked by our Handle Equipment packet. <see cref="HandleEquipment"/>
    /// </summary>
    /// <param name="a_itemName">The item name we use to find the resource model</param>
    /// <param name="a_parentName">The tranform object where the equipped item will be parented to</param>
    public void EquipItem(string a_itemName, string a_parentName)
    {
        Transform parentTransform = null;
        TransformMap.TryGetValue(a_parentName, out parentTransform);
        print("Find transform: " + a_parentName + " in transform: " + transform.name);
        if (parentTransform == null)
        {
            var foundTransform = FindDeepChild(a_parentName);
            if (foundTransform != null)
            {
                parentTransform = foundTransform;
                TransformMap.Add(a_parentName, parentTransform);
            }
        }
        if (parentTransform == null)
        {
            print("no child:" + a_parentName + " found in " + transform.gameObject);
            return;
        }
        //Destroy an existing item if we have one. We shouldn't though because unequip should remove from list..
        GameObject CurrentItemInTransform = null;
        ItemToTransformMap.TryGetValue(parentTransform, out CurrentItemInTransform);
        if (CurrentItemInTransform != null)
        {
            UnEquip(a_itemName, a_parentName);
        }
        var ObjectForEquip = GetModelForName(a_itemName);
        if (ObjectForEquip != null)
        {
            ObjectForEquip.SetActive(true);
            ItemToTransformMap[parentTransform] = Instantiate(ObjectForEquip, parentTransform);
            
        }

        print("Client is equipping item:" + a_itemName);
    }
    

    /// <summary>
    /// This is invoked when we get an equipment packet. Since we have Equip using the same invoke, we pass the same paramter requirements even tho we dont use it.
    /// We dont validate if items match on client because server is authorating the change.
    /// </summary>
    /// <param name="a_itemName">Ignored paramater used in Equip()</param>
    /// <param name="a_parentName">Transform currently holding the active item on server</param>
    public void UnEquip(string a_itemName, string a_parentName)
    {
        Transform parentTransform = null;
        TransformMap.TryGetValue(a_parentName, out parentTransform);
        if (parentTransform == null)
        {
            //doesnt exist
            return;
        }
        Destroy(ItemToTransformMap[parentTransform]);
        ItemToTransformMap[parentTransform] = null;
        
    }

    /// <summary>
    /// This function checks our resource library and see if we have a specific gameobject with this name
    /// </summary>
    /// <param name="name"></param>
    /// <returns>The game object found one or null.</returns>
    private GameObject GetModelForName(string name)
    {
        var gameObject = Resources.Load(("ItemResources/ItemModels/" + name)) as GameObject;
        if (gameObject)
        {
            return gameObject;
        }
        return null;
    }

    /// <summary>
    /// This is a function used to find a decendant. We use Brith First Search to find a child containing matching name. Case Sensative
    /// </summary>
    /// <param name="a_childName">ChildName used to match search</param>
    /// <returns>Descendant GameObject</returns>
    public Transform FindDeepChild(string a_childName)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(transform);
        while (queue.Count > 0)
        {
            var c = queue.Dequeue();
            if (c.name == a_childName)
                return c;
            foreach (Transform t in c)
                queue.Enqueue(t);
        }
        return null;
    }
}
