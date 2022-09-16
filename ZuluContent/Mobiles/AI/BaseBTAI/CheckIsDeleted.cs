namespace Server.Mobiles;

public class CheckIsDeleted : BTNode
{
    private readonly BaseCreature m_Mobile;
    
    public CheckIsDeleted(BaseCreature mobile)
    {
        m_Mobile = mobile;
    }

    public override NodeState Evaluate()
    {
        if (m_Mobile.Deleted)
        {
            State = NodeState.SUCCESS;
            return State;
        }

        State = NodeState.FAILURE;
        return State;
    }
}