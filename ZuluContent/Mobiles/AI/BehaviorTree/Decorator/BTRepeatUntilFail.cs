using System.Collections.Generic;

namespace Server.Mobiles;

public class BTRepeatUntilFail : BTNode
{
    public BTRepeatUntilFail()
    {
    }

    public BTRepeatUntilFail(BTNode child) : base(new List<BTNode> { child })
    {
    }

    public override NodeState Evaluate()
    {
        State = NodeState.RUNNING;
        
        while (State != NodeState.FAILURE)
        {
            State = Children[0].Evaluate();
        }

        return State;
    }
}