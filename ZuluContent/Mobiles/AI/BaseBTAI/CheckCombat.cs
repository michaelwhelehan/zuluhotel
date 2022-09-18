namespace Server.Mobiles;

public class CheckCombat : BTNode
{
    private readonly BTTree m_BtTree;
    
    public CheckCombat(BTTree tree)
    {
        m_BtTree = tree;
    }

    public override NodeState Evaluate()
    {
        var combat = (bool) (m_BtTree.GetData("combat") ?? false);

        if (combat)
        {
            State = NodeState.SUCCESS;
            return State;
        }

        State = NodeState.FAILURE;
        return State;
    }
}