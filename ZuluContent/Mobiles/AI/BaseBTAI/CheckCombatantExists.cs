namespace Server.Mobiles;

public class CheckCombatantExists : BTNode
{
    private readonly BaseCreature m_Mobile;
    
    public CheckCombatantExists(BaseCreature mobile)
    {
        m_Mobile = mobile;
    }

    public override NodeState Evaluate()
    {
        var combatant = m_Mobile.Combatant;

        if (combatant == null || combatant.Deleted || combatant.Map != m_Mobile.Map || !combatant.Alive)
        {
            m_Mobile.DebugSay("My combatant is gone");
            
            State = NodeState.FAILURE;
            return State;
        }

        State = NodeState.SUCCESS;
        return State;
    }
}