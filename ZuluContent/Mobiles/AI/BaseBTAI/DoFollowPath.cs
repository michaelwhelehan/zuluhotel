namespace Server.Mobiles;

public class DoFollowPath : BTNode
{
    private readonly BTTree m_BtTree;
    private readonly bool m_Run;
    
    public DoFollowPath(BTTree tree, bool run)
    {
        m_BtTree = tree;
        m_Run = run;
    }

    public override NodeState Evaluate()
    {
        var path = (PathFollower) m_BtTree.GetData("path");

        if (path?.Follow(m_Run, 1) == true)
        {
            State = NodeState.SUCCESS;
            return State;
        }

        State = NodeState.RUNNING;
        return State;
    }
}