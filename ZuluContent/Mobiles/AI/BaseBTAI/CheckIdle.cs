namespace Server.Mobiles;

public class CheckIdle : BTNode
{
    private readonly BaseCreature m_Mobile;
    
    public CheckIdle(BaseCreature mobile)
    {
        m_Mobile = mobile;
    }

    public override NodeState Evaluate()
    {
        if (m_Mobile.CheckIdle())
        {
            State = NodeState.SUCCESS;
            return State;
        }

        State = NodeState.FAILURE;
        return State;
    }
}