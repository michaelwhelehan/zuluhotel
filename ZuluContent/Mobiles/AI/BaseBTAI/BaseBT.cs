namespace Server.Mobiles;

public abstract class BaseBT : BTTree
{
    protected readonly BaseCreature Mobile;
    
    public BaseBT(BaseCreature mobile)
    {
        Mobile = mobile;
    }
}