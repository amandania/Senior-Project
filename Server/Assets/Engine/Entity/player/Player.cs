public class Player : Character
{
    private readonly string m_userName;
    private readonly string m_password;

    public PlayerSession Session { get; set; }

    private readonly IWorld m_world;
    public bool MenuOpen { get; set; }

    public Player(PlayerSession session, IWorld world)
    {
        Session = session;
        m_userName = "";
        m_password = "";
        m_world = world;
    }
    
    public void SetupGameModel()
    {
        var myModel = GetCharModel();

        var currentCombatComp = myModel.GetComponent<CombatComponent>();
        SetCombatComponent(currentCombatComp ?? myModel.AddComponent<CombatComponent>());
        UnityEngine.Debug.Log("set combat compont " + currentCombatComp);
        var currentMoveComp = myModel.GetComponent<MovementComponent>();
        SetMoveComponent(currentMoveComp ?? myModel.AddComponent<MovementComponent>());
    }

}
