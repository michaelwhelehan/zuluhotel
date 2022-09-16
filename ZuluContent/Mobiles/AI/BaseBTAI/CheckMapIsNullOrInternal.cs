namespace Server.Mobiles;

public class CheckMapIsNullOrInternal : BTNode
{
    private readonly BaseCreature m_Mobile;
    
    public CheckMapIsNullOrInternal(BaseCreature mobile)
    {
        m_Mobile = mobile;
    }

    public override NodeState Evaluate()
    {
        if (m_Mobile.Map == null || m_Mobile.Map == Map.Internal)
        {
            State = NodeState.SUCCESS;
            return State;
        }

        State = NodeState.FAILURE;
        return State;
    }
}