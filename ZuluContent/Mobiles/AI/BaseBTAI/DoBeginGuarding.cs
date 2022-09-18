using System;

namespace Server.Mobiles;

public class DoBeginGuarding : BTNode
{
    private readonly BTTree m_BtTree;
    private readonly BaseCreature m_Mobile;

    public DoBeginGuarding(BaseCreature mobile, BTTree tree)
    {
        m_Mobile = mobile;
        m_BtTree = tree;
    }

    public override NodeState Evaluate()
    {
        m_Mobile.Warmode = true;
        m_Mobile.FocusMob = null;
        m_Mobile.Combatant = null;
        m_Mobile.CurrentSpeed = m_Mobile.ActiveSpeed;

        m_BtTree.SetData("guarding", true);
        m_BtTree.SetData("nextStopGuard", Core.TickCount + (int) TimeSpan.FromSeconds(10).TotalMilliseconds);

        State = NodeState.SUCCESS;
        return State;
    }
}