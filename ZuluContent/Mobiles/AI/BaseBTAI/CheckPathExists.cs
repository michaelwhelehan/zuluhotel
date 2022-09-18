namespace Server.Mobiles;

public class CheckPathExists : BTNode
{
    private readonly BaseCreature m_Mobile;
    private readonly BTTree m_BtTree;
    
    public CheckPathExists(BaseCreature mobile, BTTree tree)
    {
        m_Mobile = mobile;
        m_BtTree = tree;
    }

    public override NodeState Evaluate()
    {
        var combatant = m_Mobile.Combatant;

        var path = (PathFollower) m_BtTree.GetData("path");

        if (path != null && path.Goal == combatant)
        {
            State = NodeState.SUCCESS;
            return State;
        }

        State = NodeState.FAILURE;
        return State;
    }
}