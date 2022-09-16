using System.Collections.Generic;

namespace Server.Mobiles;

public class BTRepeater : BTNode
{
    private int m_NumRepeats;
    
    public BTRepeater()
    {
    }

    public BTRepeater(BTNode child, int numRepeats = -1) : base(new List<BTNode> { child })
    {
        m_NumRepeats = numRepeats;
    }

    public override NodeState Evaluate()
    {
        State = NodeState.RUNNING;
        
        while (m_NumRepeats == -1 || m_NumRepeats-- > 0)
        {
            State = Children[0].Evaluate();
        }

        return State;
    }
}