using Engine.Entity.npc;

public abstract class Entity
{
    public Entity()
    {
    }

    public bool IsPlayer()
    {
        return this is Player;
    }

    public Player AsPlayer()
    {
        return ((Player)this);
    }
    public bool IsNpc()
    {
        return this is NPC;
    }

    public NPC AsNpc()
    {
        return ((NPC)this);
    }
}
