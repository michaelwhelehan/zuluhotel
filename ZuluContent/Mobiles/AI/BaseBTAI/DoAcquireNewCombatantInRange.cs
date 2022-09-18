namespace Server.Mobiles;

public class DoAcquireNewCombatantInRange : BTNode
{
    private readonly BaseCreature m_Mobile;
    
    public DoAcquireNewCombatantInRange(BaseCreature mobile)
    {
        m_Mobile = mobile;
    }

    public override NodeState Evaluate()
    {
        if (BaseBTAI.AcquireFocusMob(m_Mobile, m_Mobile.RangePerception, m_Mobile.FightMode, false, false, true))
        {
            m_Mobile.DebugSay("My move is blocked, so I am going to attack {0}", m_Mobile.FocusMob.Name);
            
            m_Mobile.Combatant = m_Mobile.FocusMob;
            m_Mobile.FocusMob = null;

            State = NodeState.SUCCESS;
            return State;
        }

        State = NodeState.FAILURE;
        return State;
    }
}