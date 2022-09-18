namespace Server.Mobiles;

public class CheckCombatantInRange : BTNode
{
    private readonly BaseCreature m_Mobile;
    private readonly int m_Multiplier;
    
    public CheckCombatantInRange(BaseCreature mobile, int multiplier = 1)
    {
        m_Mobile = mobile;
        m_Multiplier = multiplier;
    }

    public override NodeState Evaluate()
    {
        var combatant = m_Mobile.Combatant;

        if (m_Mobile.InRange(combatant, m_Mobile.RangePerception * m_Multiplier))
        {
            State = NodeState.SUCCESS;
            return State;
        }
        
        State = NodeState.FAILURE;
        return State;
    }
}