namespace Server.Mobiles;

public class DoRemoveCombatant : BTNode
{
    private readonly BaseCreature m_Mobile;
    
    public DoRemoveCombatant(BaseCreature mobile)
    {
        m_Mobile = mobile;
    }

    public override NodeState Evaluate()
    {
        m_Mobile.Combatant = null;

        State = NodeState.SUCCESS;
        return State;
    }
}