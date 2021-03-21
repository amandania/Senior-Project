using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class is use to create any server players which is of type character.
/// We inherit some base data from character such as the positions and rotations we use do send over the network
/// But we also have unique function calls that only behave striclty for an Player type. Along with some game object setup details for any combat state npcs.
/// </summary>
public class Player : Character
{
    private readonly IWorld m_world;

    //Set only on PlayerLoad and sets combat component health data
    /// <summary>
    /// <see cref="PlayerData.LoadPlayerData"/>
    /// </summary>
    private int m_maxHealth;
    private int m_currentHealth;
    public int MaxHealth { get => m_maxHealth; set => m_maxHealth = value; }
    public int CurrentHealth { get => m_currentHealth; set => m_currentHealth = value; }


    /// <summary>
    /// These variables are exposed to Unity Threads and Network threads
    /// </summary>
    public string UserName { get; set; }
    public string Password { get; set; }
    public PlayerSession Session { get; set; }
    public Hotkeys HotkeyInventory;
    public bool MenuOpen { get; set; }
    public GameObject CurrentInteractGuid;

    public Dialogue ActiveDialouge { get; set; }
    public DialogueOptions MyOptionHandle { get; set; }
    public int DialougeMessageIndex { get; set; } = 0;
    public Dictionary<string, Quest> PlayerQuests { get; set; } = new Dictionary<string, Quest>();

    /// <summary>
    /// We create a constructor for a player using the IWorld Interface because we have already decided that in our server container, we consider IWorld a single instance dependency.
    /// <see cref="NetworkManager.RegisterDependencies(Autofac.ContainerBuilder))"/>
    /// </summary>
    /// <param name="session"></param>
    /// <param name="world"></param>
    public Player(PlayerSession session, IWorld world)
    {
        Session = session;
        UserName = "";
        Password = "";
        m_world = world;
        HotkeyInventory = new Hotkeys(this, 8);
        m_currentHealth = -1;
        m_maxHealth = -1;
    }

    /// <summary>
    /// This function will setup our inital player game model based on how we want a player to start
    /// wee setup the default health data and auto equip items that need to be equipped on login here.
    /// </summary>
    public void SetupGameModel()
    {
        var myModel = GetCharModel();

        var currentCombatComp = myModel.GetComponent<CombatComponent>();
        SetCombatComponent(currentCombatComp ?? myModel.AddComponent<CombatComponent>());
        //UnityEngine.Debug.Log("set combat compont " + currentCombatComp);
        var currentMoveComp = myModel.GetComponent<MovementComponent>();
        SetMoveComponent(currentMoveComp ?? myModel.AddComponent<MovementComponent>());

        var equipment = myModel.AddComponent<Equipment>();
        SetEquipmentComponent(equipment);
        if (HotkeyInventory.LastActiveSlot != -1)
        {
            var slotItem = HotkeyInventory.ContainerItems[HotkeyInventory.LastActiveSlot];
            Debug.Log("Last active slot: " + HotkeyInventory.LastActiveSlot + " slot item: "+ slotItem.ItemName);
            Debug.Log("equip to :" + slotItem.TrasnformParentName);
            HotkeyInventory.ToggleEquip(slotItem, true);
            //equipment.EquipItem(slotItem.ItemName, slotItem.TrasnformParentName);
            //Equipment.EquipItem(HotkeyInventory.ContainerItems[HotkeyInventory.LastActiveSlot].ItemName, HotkeyInventory.ContainerItems[HotkeyInventory.LastActiveSlot].TrasnformParentName);
        }
        HotkeyInventory.RefrehsItems();
        currentCombatComp.CurrentHealth = m_currentHealth == -1 ? 100 : m_currentHealth;
        currentCombatComp.MaxHealth = m_maxHealth == -1 ? 100 : m_maxHealth;
    }

}
