using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class ExportTerrainData : ScriptableObject
{
    static Terrain terrain;



    [MenuItem("Map Exporter/Export Terrain info")]
    static void ExportTerrainInfo()
    {
        terrain = GameObject.Find("Terrain_0_0-20191004-234637").GetComponent<Terrain>();
        TerrainData data = terrain.terrainData;
        var scale = data.heightmapScale;
        var size = data.size;
        var width = data.heightmapResolution;
        var height = data.heightmapResolution;
        Debug.Log("Width*height =  " + (width * height));
        List<Vector3> list = NavMesh.CalculateTriangulation().vertices.OfType<Vector3>().ToList();
        List<Vector3> terrains = list.Where(point => point.y > 50.0f).ToList();

        Debug.Log("terrain size =  " + terrains.Count);
        TerrainDataStuff stuff = new TerrainDataStuff();
        stuff.maxLength = terrains.Count;
        stuff.x = new float[terrains.Count];
        stuff.y = new float[terrains.Count];
        stuff.z = new float[terrains.Count];
        
        for (int i = 0; i < terrains.Count; i++)
        {
            stuff.x[i] = terrains[i].x;
            stuff.y[i] = terrains[i].y;
            stuff.z[i] = terrains[i].z;
        }
        //File.WriteAllText(Application.dataPath + "/maps/" + counter + ".json", JsonUtility.ToJson(stuff));


        var bytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(stuff));
        byte[] r1;
        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream())
        {
            using (var gs = new GZipStream(mso, CompressionMode.Compress))
            {
                //msi.CopyTo(gs);
                CopyTo(msi, gs);
            }
            r1 = mso.ToArray();
        }
        File.WriteAllBytes(Application.dataPath + "/maps/" + 43 + ".dat", r1); 



    }
    public static void CopyTo(Stream src, Stream dest)
    {
        byte[] bytes = new byte[4096];

        int cnt;

        while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
        {
            dest.Write(bytes, 0, cnt);
        }
    }
        public class TerrainDataStuff {

            public float[] x;
            public float[] y;
            public float[] z;
            public int maxLength;
        }
}
