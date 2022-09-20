namespace Server.Mobiles;

public class DoMoveToCombatant<T> : BTNode<T> where T : IAIState
{
    private readonly bool m_BadStateOk;
    
    public DoMoveToCombatant(bool badStateOk)
    {
        m_BadStateOk = badStateOk;
    }

    public override NodeState Evaluate(T treeState)
    {
        var combatant = treeState.Creature.Combatant;
        
        treeState.Creature.DebugSay("Trying to move to my combatant");

        var nextMove = treeState.NextMove;
        
        var res = BaseBTAI<T>.DoMoveImpl(treeState.Creature, treeState.Creature.GetDirectionTo(combatant), ref nextMove);

        treeState.NextMove = nextMove;

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
            return NodeState.SUCCESS;
        }
        
        return NodeState.FAILURE;
    }
}