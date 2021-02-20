using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PlayerData : IPlayerDataLoader
{

    private readonly string m_filePath;

    //Collection for our files loaded
    //Used to check if we are already logged in
    private List<string> SavesLoded { get; set; }
    
    /// <summary>
    /// Created during depecendy build NetworkManager.RegisterDependencies
    /// </summary>
    public PlayerData()
    {
        m_filePath = Path.Combine(Application.dataPath, "PlayerDataSaves");
    }

    /// <summary>
    /// Triggerd by - <see cref="LoginResponsePacket.ExecutePacket"/>
    /// Load player file and read to match password (case sensative)
    /// We use Json to desirealize the PlayerSave struc
    /// </summary>
    /// <param name="a_playerName">Input Username</param>
    /// <param name="a_password">Input Password</param>
    /// <param name="a_sessionPlayer">Player Session created for packet response</param>
    /// <returns></returns>
    public bool LoadPlayerData(string a_playerName, string a_password, Player a_sessionPlayer)
    {
        a_sessionPlayer.UserName = a_playerName;
        a_sessionPlayer.Password = a_password;
        Debug.Log("check path: " + m_filePath);

        IEnumerable<string> files = Directory.GetFiles(m_filePath).Where(f => f.Equals(m_filePath + "\\" + a_playerName.ToLower() + ".json"));
        Debug.Log("count of direction matching user " + files.Count());
        if (files.Count() > 0)
        {
            //compare password validate
            var succuess = false;
            string readfile = File.ReadAllText(files.First());
            PlayerSave PlayerSave = new PlayerSave();
            var desiralizedObj = JsonConvert.DeserializeAnonymousType(readfile, PlayerSave);
            if (desiralizedObj.Username.ToLower().Equals(a_playerName) && a_password.Equals(desiralizedObj.Password)) {

                a_sessionPlayer.UserName = desiralizedObj.Username;
                a_sessionPlayer.Password = desiralizedObj.Password;
                a_sessionPlayer.CharacterLevel = desiralizedObj.PlayerLevel;
                succuess = true;
                Debug.Log("Correct creditonals given for " + a_playerName);
            }

            return succuess;

        } else
        {
            Debug.Log("New player created we will save it on logout");
            // create new file
            return true;
        }
        
    }

    /// <summary>
    /// Save data serializes our current player data
    /// Its called durinng logout <see cref="Player.HandleLogout"/>
    /// </summary>
    public void SaveData(Player a_player)
    {
        var serializeClass = new PlayerSave();
        serializeClass.Username = a_player.UserName;
        serializeClass.Password = a_player.Password;
        serializeClass.PlayerLevel = a_player.CharacterLevel;
        // serialize JSON directly to a file
        using (StreamWriter file = File.CreateText(m_filePath + "/"+a_player.UserName.ToLower() + ".json")) 
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(file, serializeClass);
            Debug.Log("wrote to file : " + m_filePath + "/" + a_player.UserName + ".json");
        }
    }


    public void Dispose()
    {

    }


    public void Start()
    {

      
    }
}
