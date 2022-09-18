namespace Server.Mobiles;

public class CheckCombatantTooFarAway : BTNode
{
    private readonly BaseCreature m_Mobile;
    
    public CheckCombatantTooFarAway(BaseCreature mobile)
    {
        m_Mobile = mobile;
    }

    public override NodeState Evaluate()
    {
        var combatant = m_Mobile.Combatant;

        if (m_Mobile.GetDistanceToSqrt(combatant) > m_Mobile.RangePerception + 1)
        {
            State = NodeState.SUCCESS;
            return State;
        }
        
        State = NodeState.FAILURE;
        return State;
    }
}