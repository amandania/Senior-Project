using System;
using UnityEngine;

public class Player : Character
{
    public string UserName { get; set; }
    public string Password { get; set; }

    public PlayerSession Session { get; set; }

    private readonly IWorld m_world;


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
    }
    
    public void SetupGameModel()
    {
        var myModel = GetCharModel();

        var currentCombatComp = myModel.GetComponent<CombatComponent>();
        SetCombatComponent(currentCombatComp ?? myModel.AddComponent<CombatComponent>());
        UnityEngine.Debug.Log("set combat compont " + currentCombatComp);
        var currentMoveComp = myModel.GetComponent<MovementComponent>();
        SetMoveComponent(currentMoveComp ?? myModel.AddComponent<MovementComponent>());

        HotkeyInventory.RefrehsItems();
    }
}
