using System;
using System.Collections.Generic;
using System.IO;
using Server.Items;

namespace Server.Mobiles;

public class DoCreatePath : BTNode
{
    private readonly BaseCreature m_Mobile;
    private readonly BTTree m_BtTree;
    
    public DoCreatePath(BaseCreature mobile, BTTree tree)
    {
        m_Mobile = mobile;
        m_BtTree = tree;
    }

    private MoveResult _MoveDirection(Direction d)
    {
        var nextMove = (long) (m_BtTree.GetData("nextMove") ?? Core.TickCount);

        var result = BaseBTAI.DoMoveImpl(m_Mobile, d, ref nextMove);
        
        m_BtTree.SetData("nextMove", nextMove);

        return result;
    }

    public override NodeState Evaluate()
    {
        var combatant = m_Mobile.Combatant;

        if (combatant == null)
        {
            State = NodeState.FAILURE;
            return State;
        }
        
        m_Mobile.DebugSay("Creating path to follow to my combatant");
        
        var path = new PathFollower(m_Mobile, combatant)
        {
            Mover = _MoveDirection
        };
        m_BtTree.SetData("path", path);

        State = NodeState.SUCCESS;
        return State;
    }
}