using System.Collections.Generic;

namespace Server.Mobiles;

public enum NodeState
{
    RUNNING,
    SUCCESS,
    FAILURE
}

public class BTNode
{
    protected NodeState State { get; set; }

    public BTNode Parent { get; set; }
    protected List<BTNode> Children { get; } = new();

    public BTNode()
    {
        Parent = null;
    }

    public BTNode(List<BTNode> children)
    {
        foreach (var child in children)
            _Attach(child);
    }

    private void _Attach(BTNode node)
    {
        node.Parent = this;
        Children.Add(node);
    }

    public virtual NodeState Evaluate()
    {
        return NodeState.FAILURE;
    }
}