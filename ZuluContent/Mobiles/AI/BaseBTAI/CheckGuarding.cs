namespace Server.Mobiles;

public class CheckGuarding : BTNode
{
    private readonly BTTree m_BtTree;
    
    public CheckGuarding(BTTree tree)
    {
        m_BtTree = tree;
    }

    public override NodeState Evaluate()
    {
        var guarding = (bool) (m_BtTree.GetData("guarding") ?? false);
        var nextStopGuard = (long) (m_BtTree.GetData("nextStopGuard") ?? 0L);

        if (guarding && Core.TickCount < nextStopGuard)
        {
            State = NodeState.SUCCESS;
            return State;
        }

        State = NodeState.FAILURE;
        return State;
    }
}