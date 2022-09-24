namespace Server.Mobiles;

public class DoMoveToTarget<T> : BTNode<T> where T : IAIState
{
    private readonly MoveToTargetType m_MoveToTarget;
    private readonly bool m_BadStateOk;
    
    public DoMoveToTarget(MoveToTargetType moveToTarget, bool badStateOk)
    {
        m_MoveToTarget = moveToTarget;
        m_BadStateOk = badStateOk;
    }

    public override NodeState Evaluate(T treeState)
    {
        var target = m_MoveToTarget switch
        {
            MoveToTargetType.Combatant => treeState.Creature.Combatant,
            MoveToTargetType.ControlTarget => treeState.Creature.ControlTarget,
            MoveToTargetType.ControlMaster => treeState.Creature.ControlMaster,
            _ => treeState.Creature.Combatant
        };
        
        treeState.Creature.DebugSay("Trying to move to my target");

        var res = treeState.Creature.DoMoveImpl(treeState.Creature.GetDirectionTo(target));

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