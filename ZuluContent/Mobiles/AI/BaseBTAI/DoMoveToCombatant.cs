namespace Server.Mobiles;

public class DoMoveToCombatant : BTNode
{
    private readonly BaseCreature m_Mobile;
    private readonly BTTree m_BtTree;
    private readonly bool m_BadStateOk;
    
    public DoMoveToCombatant(BaseCreature mobile, BTTree tree, bool badStateOk)
    {
        m_Mobile = mobile;
        m_BtTree = tree;
        m_BadStateOk = badStateOk;
    }

    public override NodeState Evaluate()
    {
        var combatant = m_Mobile.Combatant;
        
        m_Mobile.DebugSay("Trying to move to my combatant");

        var nextMove = (long) (m_BtTree.GetData("nextMove") ?? Core.TickCount);
        
        var res = BaseBTAI.DoMoveImpl(m_Mobile, m_Mobile.GetDirectionTo(combatant), ref nextMove);
        
        m_BtTree.SetData("nextMove", nextMove);

        var success = res == MoveResult.Success || res == MoveResult.SuccessAutoTurn ||
                      m_BadStateOk && res == MoveResult.BadState;

        /*if (success && (m_Mobile.Controlled || m_Mobile.Summoned) && m_Mobile.ControlOrder is OrderType.Come or OrderType.Follow)
        {
            var master = m_Mobile.GetMaster();
            if (master != null && m_Mobile.GetDistanceToSqrt(master) > 4)
            {
                m_Mobile.MoveToWorld(master.Location, master.Map);
            }
        }*/

        if (success)
        {
            State = NodeState.SUCCESS;
            return State;
        }

        State = NodeState.FAILURE;
        return State;
    }
}