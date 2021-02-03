public class Player : Character
{
    public PlayerSession Session { get; set; }

    private readonly IWorld m_world;
    public string Username { get; set; }
    public string Password { get; set; }
    public bool MenuOpen { get; set; }

    public Player(PlayerSession session, IWorld world)
    {
        Session = session;
        Password = "";
        Username = "";

        m_world = world;
    }
    
    public void SetupGameModel()
    {
        var myModel = GetCharModel();

        var currentCombatComp = myModel.GetComponent<CombatComponent>();
        SetCombatComponent(currentCombatComp ?? myModel.AddComponent<CombatComponent>());

        var currentMoveComp = myModel.GetComponent<MovementComponent>();
        SetMoveComponent(currentMoveComp ?? myModel.AddComponent<MovementComponent>());
    }

}
