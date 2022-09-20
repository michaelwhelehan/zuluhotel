namespace Server.Mobiles;

public class CheckMoveTo<T> : BTNode<T> where T : IAIState
{
    public override NodeState Evaluate(T treeState)
    {
        var combatant = treeState.Creature.Combatant;
        
        if (treeState.Creature.Deleted || treeState.Creature.DisallowAllMoves || combatant == null || combatant.Deleted)
        {
            return NodeState.FAILURE;
        }
        
        return NodeState.SUCCESS;
    }
}