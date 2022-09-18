namespace Server.Mobiles;

public class DoClearPath : BTNode
{
    private readonly BTTree m_BtTree;

    public DoClearPath(BTTree tree)
    {
        m_BtTree = tree;
    }

    public override NodeState Evaluate()
    {
        m_BtTree.ClearData("path");

        State = NodeState.SUCCESS;
        return State;
    }
}