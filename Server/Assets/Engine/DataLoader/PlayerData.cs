using System.IO;
using UnityEngine;

public class PlayerData : IPlayerDataLoader
{

    private readonly string m_filePath;

    public PlayerData()
    {
        m_filePath = Path.Combine(Application.dataPath, "Assets/PlayerData");
    }

    public void Dispose()
    {

    }

    public void Start()
    {
        Debug.Log("file path: " + m_filePath);
    }
}
