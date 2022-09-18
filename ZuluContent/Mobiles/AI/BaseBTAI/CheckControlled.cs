namespace Server.Mobiles;

public class CheckControlled : BTNode
{
    private readonly BaseCreature m_Mobile;
    
    public CheckControlled(BaseCreature mobile)
    {
        m_Mobile = mobile;
    }

    public override NodeState Evaluate()
    {
        if (m_Mobile.Controlled)
        {
            State = NodeState.SUCCESS;
            return State;
        }

        State = NodeState.FAILURE;
        return State;
    }
}