namespace Server.Mobiles;

public class DoBardPacified : BTNode
{
    private readonly BaseCreature m_Mobile;
    
    public DoBardPacified(BaseCreature mobile)
    {
        m_Mobile = mobile;
    }

    public override NodeState Evaluate()
    {
        if (Core.Now < m_Mobile.BardEndTime)
        {
            m_Mobile.DebugSay("I am pacified, I wait");
            m_Mobile.Combatant = null;
            m_Mobile.Warmode = false;

            State = NodeState.RUNNING;
            return State;
        }
        
        m_Mobile.DebugSay("I'm not pacified any longer");
        m_Mobile.BardPacified = false;

        State = NodeState.SUCCESS;
        return State;
    }
}