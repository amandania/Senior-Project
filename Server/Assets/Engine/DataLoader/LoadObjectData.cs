using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadObjectData : MonoBehaviour {


    private Dictionary<int, long> _objectIndexData = new Dictionary<int, long>();
    private BinaryReader _objectDataFileReader;
    public List<GameObject> mapGameObjects;

    public GameObject clone;

    public Transform Parent;

    // Use this for initialization
    void Start() {
        _objectDataFileReader = new BinaryReader(File.Open(Path.Combine(Application.dataPath, "MapData/object.data"), FileMode.Open));

        //load objectId indexes
        using (var streamReader = new BinaryReader(File.Open(Path.Combine(Application.dataPath, "MapData/object.idx"), FileMode.Open)))
        {
            while (streamReader.BaseStream.Position < streamReader.BaseStream.Length)
            {
                var objectId = streamReader.ReadInt32();
                var position = streamReader.ReadInt64();
                _objectIndexData.Add(objectId, position);
            }
        }
        mapGameObjects = new List<GameObject>(_objectIndexData.Count);
       
    }

    public void LoadObject(int objectId)
    {
        lock (_objectDataFileReader)
        {
            if (mapGameObjects.Count > objectId)
            {
                Debug.Log("FAULT OBJECT WAS TRYING TO LOAD but its loaded already");
                return;
            }
            GameObject obj = Instantiate(clone);
            var position = _objectIndexData[objectId];
            _objectDataFileReader.BaseStream.Position = position;
            var transformX = _objectDataFileReader.ReadSingle();
            var transformY = _objectDataFileReader.ReadSingle();
            var transformZ = _objectDataFileReader.ReadSingle();

            obj.transform.position = new Vector3(transformX, transformY, transformZ);

            var objectSizeX = _objectDataFileReader.ReadSingle();
            var objectSizeY = _objectDataFileReader.ReadSingle();
            var objectSizeZ = _objectDataFileReader.ReadSingle();


            obj.transform.localScale = new Vector3(objectSizeX, objectSizeY, objectSizeZ);

            var colliderType = _objectDataFileReader.ReadInt32();
            var radius = 0f;
            var height = 0f;
            if(colliderType != -1)
            {
                var colliderCenterX = _objectDataFileReader.ReadSingle();
                var colliderCenterY = _objectDataFileReader.ReadSingle();
                var colliderCenterZ = _objectDataFileReader.ReadSingle();

                switch(colliderType)
                {
                    case 1:
                        radius = _objectDataFileReader.ReadSingle();
                        obj.AddComponent<SphereCollider>();
                        break;
                    case 2:
                        radius = _objectDataFileReader.ReadSingle();
                        height = _objectDataFileReader.ReadSingle();
                        obj.AddComponent<CapsuleCollider>();
                        break;
                    case 3:
                        obj.AddComponent<BoxCollider>();
                        break;
                }
                if (colliderType == 1)
                {
                    obj.GetComponent<SphereCollider>().center = new Vector3(colliderCenterX, colliderCenterY, colliderCenterZ);
                    obj.GetComponent<SphereCollider>().radius = radius;
                }
                else if (colliderType == 2)
                {
                    obj.GetComponent<CapsuleCollider>().center = new Vector3(colliderCenterX, colliderCenterY, colliderCenterZ);
                    obj.GetComponent<CapsuleCollider>().radius = radius;
                    obj.GetComponent<CapsuleCollider>().height = height;
                }
                else
                {
                    //boxcollider
                    obj.GetComponent<BoxCollider>().center = new Vector3(colliderCenterX, colliderCenterY, colliderCenterZ);
                }
                //Instantiate(obj);
                obj.transform.parent = Parent;

            } else
            {
                //Instantiate(obj);
                obj.transform.parent = Parent;
            }

        }
    }
    // Update is called once per frame
    void Update () {
		
	}



}
