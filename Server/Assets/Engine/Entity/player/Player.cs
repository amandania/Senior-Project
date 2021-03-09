using System;
using UnityEngine;

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

    public void SetupGameModel()
    {
        var myModel = GetCharModel();

        var currentCombatComp = myModel.GetComponent<CombatComponent>();
        SetCombatComponent(currentCombatComp ?? myModel.AddComponent<CombatComponent>());
        //UnityEngine.Debug.Log("set combat compont " + currentCombatComp);
        var currentMoveComp = myModel.GetComponent<MovementComponent>();
        SetMoveComponent(currentMoveComp ?? myModel.AddComponent<MovementComponent>());
        HotkeyInventory.RefrehsItems();
        currentCombatComp.CurrentHealth = m_currentHealth == -1 ? 100 : m_currentHealth;
        currentCombatComp.MaxHealth = m_maxHealth == -1 ? 100 : m_maxHealth;

        
    }
}
