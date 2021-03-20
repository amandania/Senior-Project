using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class is used for Player session saving and loading. We serialize the data and save it a .json text file. We also deserialize it the same way and initalize the respective player session channel data
/// This class is also a dependecy build which means we can pass it as a instance container in any other Constructors in the container build. <seealso cref="NetworkManager.RegisterDependencies(Autofac.ContainerBuilder)"/>
/// </summary>
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
    /// Load player file and compare username and pasword from the file to input. The password is case sensative.
    /// We use Json to desirealize the save file <see cref="PlayerSave"/>
    /// </summary>
    /// <param name="a_playerName">Input Username</param>
    /// <param name="a_password">Input Password</param>
    /// <param name="a_sessionPlayer">Player Session created for packet response</param>
    /// <returns> true, if credientials matched or new player, false otherwise.</returns>
    public bool LoadPlayerData(string a_playerName, string a_password, Player a_sessionPlayer)
    {
        a_sessionPlayer.UserName = a_playerName;
        a_sessionPlayer.Password = a_password;
        //Debug.Log("check path: " + m_filePath);

        IEnumerable<string> files = Directory.GetFiles(m_filePath).Where(f => f.Equals(m_filePath + "\\" + a_playerName.ToLower() + ".json"));
        //Debug.Log("count of direction matching user " + files.Count());
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
                a_sessionPlayer.HotkeyInventory.LastActiveSlot = desiralizedObj.CurrentSlotEquipped;
                if (desiralizedObj.HotkeyItems.Length > 0)
                {
                    a_sessionPlayer.HotkeyInventory.ContainerItems.Clear();
                    a_sessionPlayer.HotkeyInventory.ContainerItems.AddRange(desiralizedObj.HotkeyItems);
                    a_sessionPlayer.HotkeyInventory.RefrehsItems();
                }
                if (desiralizedObj.PlayerQuests != null && desiralizedObj.PlayerQuests.Length > 0)
                {
                    a_sessionPlayer.PlayerQuests.Clear();
                    var QuestList = desiralizedObj.PlayerQuests;
                    foreach (Quest q in QuestList)
                    {
                        Debug.Log("has qust load step: " + q.CurrentQuestStep);
                        a_sessionPlayer.PlayerQuests.Add(q.QuestName, new Quest(q.QuestName, q.MaxQuestStep, q.CurrentQuestStep));
                        a_sessionPlayer.PlayerQuests[q.QuestName].Claimed = q.Claimed;
                    }
                }
                if (desiralizedObj.CurrentSlotEquipped != -1)
                {
                    a_sessionPlayer.HotkeyInventory.LastActiveSlot = desiralizedObj.CurrentSlotEquipped;
                }
                succuess = true;
                //Debug.Log("Correct creditonals given for " + a_playerName);
            }

            return succuess;

        } else
        {
            //Debug.Log("New player created we will save it on logout");
            // create new file
            return true;
        }
        
    }

    /// <summary>
    /// Save data serializes our current player data in Json format <see cref="PlayerSave"/>
    /// Its called durinng logout <see cref="Player.HandleLogout"/>
    /// </summary>
    public void SaveData(Player a_player)
    {
        var serializeClass = new PlayerSave();
        serializeClass.Username = a_player.UserName;
        serializeClass.Password = a_player.Password;
        serializeClass.PlayerLevel = a_player.CharacterLevel;
        serializeClass.HotkeyItems = a_player.HotkeyInventory.ContainerItems.ToArray();
        serializeClass.PlayerQuests = new Quest[a_player.PlayerQuests.Count];
        int quest = 0;
        foreach(Quest q in a_player.PlayerQuests.Values)
        {
            Debug.Log("save quest step:" + q.CurrentQuestStep);
            serializeClass.PlayerQuests[quest] = q;
        }
        serializeClass.CurrentHealth = a_player.CombatComponent.CurrentHealth;
        serializeClass.MaxHealth = a_player.CombatComponent.MaxHealth;
        serializeClass.CurrentSlotEquipped = a_player.HotkeyInventory.LastActiveSlot;

        //Debug.Log(a_player.CombatComponent.MaxHealth + " max health on log");
        // serialize JSON directly to a file
        using (StreamWriter file = File.CreateText(m_filePath + "/"+a_player.UserName.ToLower() + ".json")) 
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            serializer.Serialize(file, serializeClass);
            //Debug.Log("wrote to file : " + m_filePath + "/" + a_player.UserName + ".json");
        }
    }
    
    /// <summary>
    /// Required for IDisposable
    /// Acts as a deconstructor
    /// </summary>
    public void Dispose()
    {
        SavesLoded.Clear();
    }

    //required startable class
    public void Start()
    {

      
    }
}
