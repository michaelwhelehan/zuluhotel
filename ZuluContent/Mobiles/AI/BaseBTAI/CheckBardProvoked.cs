namespace Server.Mobiles;

public class CheckBardProvoked : BTNode
{
    private readonly BaseCreature m_Mobile;
    
    public CheckBardProvoked(BaseCreature mobile)
    {
        m_Mobile = mobile;
    }

    public override NodeState Evaluate()
    {
        if (m_Mobile.BardProvoked)
        {
            State = NodeState.SUCCESS;
            return State;
        }

        State = NodeState.FAILURE;
        return State;
    }
}