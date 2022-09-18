namespace Server.Mobiles;

public class CheckCombatantInAttackRange : BTNode
{
    private readonly BaseCreature m_Mobile;
    
    public CheckCombatantInAttackRange(BaseCreature mobile)
    {
        m_Mobile = mobile;
    }

    public override NodeState Evaluate()
    {
        var combatant = m_Mobile.Combatant;

        if (m_Mobile.InRange(combatant, m_Mobile.RangeFight))
        {
            State = NodeState.SUCCESS;
            return State;
        }
        
        State = NodeState.FAILURE;
        return State;
    }
}