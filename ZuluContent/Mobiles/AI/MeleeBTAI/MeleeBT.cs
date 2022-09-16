using System.Collections.Generic;

namespace Server.Mobiles;

public class MeleeBT : BTTree
{
    private BaseBTAI m_AI;
    
    public MeleeBT(BaseBTAI ai)
    {
        m_AI = ai;
    }
    
    protected override BTNode SetupTree()
    {
        var root = new BTSequence(new List<BTNode>
        {
            new BTInverter(new CheckIsDeleted(m_AI.Mobile)),
            new BTInverter(new CheckMapIsNullOrInternal(m_AI.Mobile)),
            new BTInverter(new CheckMapSectorInactive(m_AI.Mobile)),
            new BTSelector(new List<BTNode>
            {
                new BTSequence(new List<BTNode>
                {
                    new CheckBardPacified(m_AI.Mobile),
                })
            })
        });

        return root;
    }
}