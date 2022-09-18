using System;

namespace Server.Mobiles;

public class DoBeginCombat : BTNode
{
    private readonly BTTree m_BtTree;
    private readonly BaseCreature m_Mobile;

    public DoBeginCombat(BaseCreature mobile, BTTree tree)
    {
        m_Mobile = mobile;
        m_BtTree = tree;
    }

    public override NodeState Evaluate()
    {
        m_Mobile.Warmode = true;
        m_Mobile.FocusMob = null;
        m_Mobile.CurrentSpeed = m_Mobile.ActiveSpeed;

        m_BtTree.SetData("combat", true);

        m_Mobile.DebugSay("Beginning combat");

        State = NodeState.SUCCESS;
        return State;
    }
}