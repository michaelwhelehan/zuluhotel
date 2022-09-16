using System.Collections.Generic;

namespace Server.Mobiles;

public class BTSucceeder : BTNode
{
    public BTSucceeder()
    {
    }
    
    public BTSucceeder(BTNode child) : base(new List<BTNode> { child }) { }

    public override NodeState Evaluate()
    {
        State = Children[0].Evaluate();

        return State == NodeState.RUNNING ? NodeState.RUNNING : NodeState.SUCCESS;
    }
}