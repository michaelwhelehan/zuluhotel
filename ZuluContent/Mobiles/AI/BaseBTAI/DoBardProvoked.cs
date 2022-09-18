using System;

namespace Server.Mobiles;

public class DoBardProvoked : BTNode
{
    private readonly BaseCreature m_Mobile;
    
    public DoBardProvoked(BaseCreature mobile)
    {
        m_Mobile = mobile;
    }

    public override NodeState Evaluate()
    {
        if (DateTime.Now >= m_Mobile.BardEndTime && (m_Mobile.BardMaster == null || m_Mobile.BardMaster.Deleted ||
                                                     m_Mobile.BardMaster.Map != m_Mobile.Map ||
                                                     m_Mobile.GetDistanceToSqrt(m_Mobile.BardMaster) >
                                                     m_Mobile.RangePerception))
        {
            m_Mobile.DebugSay("I have lost my provoker");
            m_Mobile.BardProvoked = false;
            m_Mobile.BardMaster = null;
            m_Mobile.BardTarget = null;

            m_Mobile.Combatant = null;
            m_Mobile.Warmode = false;

            State = NodeState.FAILURE;
            return State;
        }
        
        if (m_Mobile.BardTarget == null || m_Mobile.BardTarget.Deleted ||
            m_Mobile.BardTarget.Map != m_Mobile.Map ||
            m_Mobile.GetDistanceToSqrt(m_Mobile.BardTarget) > m_Mobile.RangePerception)
        {
            m_Mobile.DebugSay("I have lost my provoke target");
            m_Mobile.BardProvoked = false;
            m_Mobile.BardMaster = null;
            m_Mobile.BardTarget = null;

            m_Mobile.Combatant = null;
            m_Mobile.Warmode = false;
            
            State = NodeState.FAILURE;
            return State;
        }
        
        m_Mobile.Combatant = m_Mobile.BardTarget;
        m_Mobile.Warmode = true;
        m_Mobile.FocusMob = null;
        m_Mobile.CurrentSpeed = m_Mobile.ActiveSpeed;

        State = NodeState.RUNNING;
        return State;
    }
}