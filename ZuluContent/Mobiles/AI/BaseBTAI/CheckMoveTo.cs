namespace Server.Mobiles;

public class CheckMoveTo : BTNode
{
    private readonly BaseCreature m_Mobile;
    
    public CheckMoveTo(BaseCreature mobile)
    {
        m_Mobile = mobile;
    }

    public override NodeState Evaluate()
    {
        var combatant = m_Mobile.Combatant;
        
        if (m_Mobile.Deleted || m_Mobile.DisallowAllMoves || combatant == null || combatant.Deleted)
        {
            State = NodeState.FAILURE;
            return State;
        }

        State = NodeState.SUCCESS;
        return State;
    }
}