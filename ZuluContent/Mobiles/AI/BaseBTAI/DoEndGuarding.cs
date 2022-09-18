using System;

namespace Server.Mobiles;

public class DoEndGuarding : BTNode
{
    private readonly BTTree m_BtTree;

    public DoEndGuarding(BTTree tree)
    {
        m_BtTree = tree;
    }

    public override NodeState Evaluate()
    {
        m_BtTree.ClearData("guarding");
        m_BtTree.ClearData("nextStopGuard");

        State = NodeState.SUCCESS;
        return State;
    }
}