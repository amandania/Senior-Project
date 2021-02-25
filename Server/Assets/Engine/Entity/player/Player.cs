using System;

public class Player : Character
{
    public string UserName { get; set; }
    public string Password { get; set; }

    public PlayerSession Session { get; set; }

    private readonly IWorld m_world;
    private readonly Hotkeys m_inventroy;

    public bool MenuOpen { get; set; }

    public string CurrentInteractGuid;

    public Player(PlayerSession session, IWorld world)
    {
        Session = session;
        UserName = "";
        Password = "";
        m_world = world;
        m_inventroy = new Hotkeys(this, 9);
    }
    
    public void SetupGameModel()
    {
        var myModel = GetCharModel();

        var currentCombatComp = myModel.GetComponent<CombatComponent>();
        SetCombatComponent(currentCombatComp ?? myModel.AddComponent<CombatComponent>());
        UnityEngine.Debug.Log("set combat compont " + currentCombatComp);
        var currentMoveComp = myModel.GetComponent<MovementComponent>();
        SetMoveComponent(currentMoveComp ?? myModel.AddComponent<MovementComponent>());

        m_inventroy.RefrehsItems();
    }
}
