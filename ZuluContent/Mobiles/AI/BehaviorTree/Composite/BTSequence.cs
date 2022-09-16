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
        var anyChildIsRunning = false;

        foreach (var node in Children)
            switch (node.Evaluate())
            {
                case NodeState.FAILURE:
                    State = NodeState.FAILURE;
                    return State;
                case NodeState.SUCCESS:
                    continue;
                case NodeState.RUNNING:
                    anyChildIsRunning = true;
                    continue;
                default:
                    State = NodeState.SUCCESS;
                    return State;
            }

        State = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
        return State;
    }
}