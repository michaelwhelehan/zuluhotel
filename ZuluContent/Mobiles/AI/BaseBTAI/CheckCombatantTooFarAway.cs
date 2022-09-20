namespace Server.Mobiles;

public class CheckCombatantTooFarAway<T> : BTNode<T> where T : IAIState
{
    public override NodeState Evaluate(T treeState)
    {
        var combatant = treeState.Creature.Combatant;

        if (treeState.Creature.GetDistanceToSqrt(combatant) > treeState.Creature.RangePerception + 1)
        {
            return NodeState.SUCCESS;
        }
        
        return NodeState.FAILURE;
    }
}