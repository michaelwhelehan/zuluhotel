using System.Collections.Generic;

namespace Server.Mobiles;

public class BTInverter : BTNode
{
    public BTInverter()
    {
    }
    
    public BTInverter(BTNode child) : base(new List<BTNode> { child }) { }

    public override NodeState Evaluate()
    {
        State = Children[0].Evaluate();

        return State switch
        {
            NodeState.FAILURE => NodeState.SUCCESS,
            NodeState.SUCCESS => NodeState.FAILURE,
            _ => State
        };
    }
}