public class Player : Character
{
    public PlayerSession Session { get; set; }

    private readonly IWorld m_world;
    private string Username { get; set; }
    private string Password { get; }
    private bool Sprinting { get; set; } = false;

    public Player(PlayerSession session, IWorld world)
    {
        m_world = world;
        Session = session;
        Password = "";
        Username = "";
        Sprinting = false;
    }

    public string GetUserName()
    {
        return Username;
    }
    public void SetUserName(string a_user)
    {
        Username = a_user;
    }
    public string GetPassword()
    {
        return Password;
    }

    public bool IsSprinting()
    {
        return Sprinting;
    }

    public void SetSprinting(bool a_active)
    {
        Sprinting = a_active;
    }

    public void SetupGameModel()
    {
        var myModel = GetCharModel();

        var currentCombatComp = myModel.GetComponent<CombatComponent>();
        CombatComponent = currentCombatComp ?? myModel.AddComponent<CombatComponent>();

        var currentMoveComp = myModel.GetComponent<MovementComponent>();
        MovementComponent = currentMoveComp ?? myModel.AddComponent<MovementComponent>();
    }

}
