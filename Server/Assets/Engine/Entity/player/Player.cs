using Engine.Interfaces;

public class Player : Character
{
    public PlayerSession m_session { get; set; }

				private readonly IWorld m_world;
				private string m_username { get; set; }
				private string m_password { get; }
				private bool m_isSprinting { get; set; } = false;

    public Player(PlayerSession session, IWorld world)
    {
        m_session = session;
        m_world = world;
								m_password = "";
								m_username = "";
								m_isSprinting = false;
    }

    public void Process()
    {

    }

				public string GetUserName()
				{
								return m_username;
				}
				public void SetUserName(string a_user)
				{
								m_username = a_user;
				}
				public string GetPassword()
				{
								return m_password;
				}

				public bool IsSprinting()
				{
								return m_isSprinting;
				}

				public void SetSprinting(bool a_active)
				{
								m_isSprinting = a_active;
				}

				public void SetupGameModel()
				{
								var myModel = GetCharModel();

								var currentCombatComp = myModel.GetComponent<CombatComponent>();
								SetCombatComponent(currentCombatComp == null ? myModel.AddComponent<CombatComponent>() : currentCombatComp);

								var currentMoveComp = myModel.GetComponent<MovementComponent>();
								SetMoveComponent(currentMoveComp == null ? myModel.AddComponent<MovementComponent>() : currentMoveComp);
				}
				
}
