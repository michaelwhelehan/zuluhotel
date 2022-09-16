namespace Server.Mobiles;

public class CheckBardPacified : BTNode
{
    private readonly BaseCreature m_Mobile;
    
    public CheckBardPacified(BaseCreature mobile)
    {
        m_Mobile = mobile;
    }

    public override NodeState Evaluate()
    {
        if (m_Mobile.BardPacified)
        {
            State = NodeState.SUCCESS;
            return State;
        }

        State = NodeState.FAILURE;
        return State;
    }
}