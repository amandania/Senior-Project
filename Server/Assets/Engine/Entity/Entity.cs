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
 
}
