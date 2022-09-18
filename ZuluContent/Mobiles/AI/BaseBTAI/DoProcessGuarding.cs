namespace Server.Mobiles;

public class DoProcessGuarding : BTNode
{
    private readonly BTTree m_BtTree;

    public DoProcessGuarding(BTTree tree)
    {
        m_BtTree = tree;
    }

    public override NodeState Evaluate()
    {
        var guarding = (bool) (m_BtTree.GetData("guarding") ?? false);
        var nextStopGuard = (long) (m_BtTree.GetData("nextStopGuard") ?? 0L);
        
        if (guarding && Core.TickCount < nextStopGuard)
        {
            State = NodeState.RUNNING;
            return State;
        }

        State = NodeState.SUCCESS;
        return State;
    }
}