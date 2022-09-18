namespace Server.Mobiles;

public class DoEndCombat : BTNode
{
    private readonly BTTree m_BtTree;

    public DoEndCombat(BTTree tree)
    {
        m_BtTree = tree;
    }

    public override NodeState Evaluate()
    {
        m_BtTree.ClearData("combat");

        State = NodeState.SUCCESS;
        return State;
    }
}