using System.Collections.Generic;

namespace Server.Mobiles;

public class BTSelector : BTNode
{
    public BTSelector()
    {
    }

    public BTSelector(List<BTNode> children) : base(children)
    {
    }

    public override NodeState Evaluate()
    {
        foreach (var node in Children)
            switch (node.Evaluate())
            {
                case NodeState.FAILURE:
                    continue;
                case NodeState.SUCCESS:
                    State = NodeState.SUCCESS;
                    return State;
                case NodeState.RUNNING:
                    State = NodeState.RUNNING;
                    return State;
                default:
                    continue;
            }

        State = NodeState.FAILURE;
        return State;
    }
}