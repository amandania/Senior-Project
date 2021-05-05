using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ExportGameObjectData : ScriptableObject
{
    [MenuItem("Map Exporter/Export Gameobject Data")]
    private static void ExportGameObjectInfo()
    {
        List<GameObject> children = new List<GameObject>();
        var mainMap = GameObject.Find("MainMap");

        int count = mainMap.transform.childCount;
        int totalGameObjects = 0;


        for (int i = 0; i < count; i++)
        {
            totalGameObjects += mainMap.transform.GetChild(i).transform.childCount;
            foreach (Transform t in mainMap.transform.GetChild(i).transform)
            {
                children.Add(t.gameObject);


            }
        }

        //Debug.Log("Total Objects = " + totalGameObjects);
        //Debug.Log("Total Objects = " + children.Count);

        List<ExportData> data = new List<ExportData>(totalGameObjects);
        for (int i = 0; i < totalGameObjects; i++)
        {


            float transformX = children[i].transform.position.x;
            float transformY = children[i].transform.position.y;
            float transformZ = children[i].transform.position.z;

            float objectSizeX = children[i].transform.localScale.x;
            float objectSizeY = children[i].transform.localScale.y;
            float objectSizeZ = children[i].transform.localScale.z;



            int colliderType = -1;
            Collider collider = children[i].transform.GetComponent<Collider>();
            if (collider != null && collider.GetType() != typeof(MeshCollider))
            {


                float radius = 0;
                float height = 0;
                colliderType = (collider.GetType() == typeof(SphereCollider) ? 1 : collider.GetType() == typeof(CapsuleCollider) ? 2 : collider.GetType() == typeof(BoxCollider) ? 3 : -1);
                if (colliderType == 1)
                {
                    radius = children[i].transform.GetComponent<SphereCollider>().radius;
                    float colliderCenterX = children[i].transform.GetComponent<SphereCollider>().center.x;
                    float colliderCenterY = children[i].transform.GetComponent<SphereCollider>().center.y;
                    float colliderCenterZ = children[i].transform.GetComponent<SphereCollider>().bounds.center.z;
                    // we just add the radius
                    data.Add(new ExportData(i, transformX, transformY, transformZ, objectSizeX, objectSizeY, objectSizeZ, colliderCenterX, colliderCenterY, colliderCenterZ, colliderType, radius));

                }
                else if (colliderType == 2)
                {

                    float colliderCenterX = children[i].transform.GetComponent<CapsuleCollider>().center.x;
                    float colliderCenterY = children[i].transform.GetComponent<CapsuleCollider>().center.y;
                    float colliderCenterZ = children[i].transform.GetComponent<CapsuleCollider>().bounds.center.z;
                    radius = children[i].transform.GetComponent<CapsuleCollider>().radius;
                    height = children[i].transform.GetComponent<CapsuleCollider>().height;
                    // we just add the radius
                    data.Add(new ExportData(i, transformX, transformY, transformZ, objectSizeX, objectSizeY, objectSizeZ, colliderCenterX, colliderCenterY, colliderCenterZ, colliderType, radius, height));

                }
                else if (colliderType == 3)
                {
                    float colliderCenterX = children[i].transform.GetComponent<BoxCollider>().center.x;
                    float colliderCenterY = children[i].transform.GetComponent<BoxCollider>().center.y;
                    float colliderCenterZ = children[i].transform.GetComponent<BoxCollider>().bounds.center.z;
                    data.Add(new ExportData(i, transformX, transformY, transformZ, objectSizeX, objectSizeY, objectSizeZ, colliderCenterX, colliderCenterY, colliderCenterZ, colliderType));

                }
            }
            else
            {
                data.Add(new ExportData(i, transformX, transformY, transformZ, colliderType));
            }

        }
        Debug.Log("Finished export of " + data.Count + " objects.");



        Dictionary<int, long> indexList = new Dictionary<int, long>();


        using (var streamWriter = new BinaryWriter(File.Create(Path.Combine(Application.dataPath, "MapData/object.data"))))
        {

            for (int objId = 0; objId < children.Count; objId++)
            {
                var node = data[objId];
                indexList.Add(objId, streamWriter.BaseStream.Position);
                streamWriter.Write(node.transformX);
                streamWriter.Write(node.transformY);
                streamWriter.Write(node.transformZ);

                streamWriter.Write(node.objectSizeX);
                streamWriter.Write(node.objectSizeY);
                streamWriter.Write(node.objectSizeZ);

                streamWriter.Write(node.colliderType);
                if (node.colliderType != -1)
                {
                    streamWriter.Write(node.colliderCenterX);
                    streamWriter.Write(node.colliderCenterY);
                    streamWriter.Write(node.colliderCenterZ);
                    switch (node.colliderType)
                    {
                        case 1:
                            //radius only
                            streamWriter.Write(node.radius);
                            break;
                        case 2:
                            //capsul gets height and radius
                            streamWriter.Write(node.radius);
                            streamWriter.Write(node.height);
                            break;
                    }
                }
            }
        }

        using (var streamWriter = new BinaryWriter(File.Create(Path.Combine(Application.dataPath, "MapData/object.idx"))))
        {
            for (int i = 0; i < indexList.Count; i++)
            {

                var objectId = i;
                var pos = indexList[objectId];
                streamWriter.Write(objectId);
                streamWriter.Write(pos);
            }
        }

    }

    public class ExportData
    {
        public int objectId;
        public float transformX;
        public float transformY;
        public float transformZ;
        public float objectSizeX;
        public float objectSizeY;
        public float objectSizeZ;

        public float colliderCenterX;
        public float colliderCenterY;
        public float colliderCenterZ;

        public int colliderType;
        public float radius;
        public float height;

        public ExportData(int objectId, float transformX, float transformY, float transformZ, int colliderType)
        {
            this.objectId = objectId;
            this.transformX = transformX;
            this.transformY = transformY;
            this.transformZ = transformZ;
            this.colliderType = colliderType;
        }

        public ExportData(int objectId, float transformX, float transformY, float transformZ,
                        float objectSizeX, float objectSizeY, float objectSizeZ,
                        float colliderCenterX, float colliderCenterY, float colliderCenterZ, int colliderType)
        {
            this.objectId = objectId;
            this.transformX = transformX;
            this.transformY = transformY;
            this.transformZ = transformZ;
            this.objectSizeX = objectSizeX;
            this.objectSizeY = objectSizeY;
            this.objectSizeZ = objectSizeZ;

            this.colliderCenterX = colliderCenterX;
            this.colliderCenterY = colliderCenterY;
            this.colliderCenterZ = colliderCenterZ;
            this.colliderType = colliderType;

        }
        public ExportData(int objectId, float transformX, float transformY, float transformZ,
                    float objectSizeX, float objectSizeY, float objectSizeZ,
                    float colliderCenterX, float colliderCenterY, float colliderCenterZ,
                    int colliderType, float radius, float height)
        {
            this.objectId = objectId;
            this.transformX = transformX;
            this.transformY = transformY;
            this.transformZ = transformZ;
            this.objectSizeX = objectSizeX;
            this.objectSizeY = objectSizeY;
            this.objectSizeZ = objectSizeZ;

            this.colliderCenterX = colliderCenterX;
            this.colliderCenterY = colliderCenterY;
            this.colliderCenterZ = colliderCenterZ;
            this.colliderType = colliderType;

        }
        public ExportData(int objectId, float transformX, float transformY, float transformZ,
                float objectSizeX, float objectSizeY, float objectSizeZ,
                float colliderCenterX, float colliderCenterY, float colliderCenterZ,
                int colliderType, float radius)
        {
            this.objectId = objectId;
            this.transformX = transformX;
            this.transformY = transformY;
            this.transformZ = transformZ;
            this.objectSizeX = objectSizeX;
            this.objectSizeY = objectSizeY;
            this.objectSizeZ = objectSizeZ;

            this.colliderCenterX = colliderCenterX;
            this.colliderCenterY = colliderCenterY;
            this.colliderCenterZ = colliderCenterZ;
            this.colliderType = colliderType;

        }
    }

}
