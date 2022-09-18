using System.Collections.Generic;

namespace Server.Mobiles;

public class BTSequence : BTNode
{
    public BTSequence()
    {
    }

    public BTSequence(List<BTNode> children) : base(children)
    {
    }

    public override NodeState Evaluate()
    {
        foreach (var node in Children)
            switch (node.Evaluate())
            {
                case NodeState.SUCCESS:
                    continue;
                case NodeState.FAILURE:
                    State = NodeState.FAILURE;
                    return State;
                case NodeState.RUNNING:
                    State = NodeState.RUNNING;
                    return State;
                default:
                    State = NodeState.SUCCESS;
                    return State;
            }

        State = NodeState.SUCCESS;
        return State;
    }
}