namespace Server.Mobiles;

public class DoCreatePath<T> : BTNode<T> where T : IAIState
{
    private static MoveResult _MoveDirection(Direction d, T treeState)
    {
        var nextMove = treeState.NextMove;

        var result = BaseBTAI<T>.DoMoveImpl(treeState.Creature, d, ref nextMove);

        treeState.NextMove = nextMove;

        return result;
    }

    public override NodeState Evaluate(T treeState)
    {
        var combatant = treeState.Creature.Combatant;

        if (combatant == null)
        {
            return NodeState.FAILURE;
        }
        
        treeState.Creature.DebugSay("Creating path to follow to my combatant");
        
        var path = new PathFollower(treeState.Creature, combatant)
        {
            Mover = (d) => _MoveDirection(d, treeState)
        };
        treeState.Path = path;
        
        return NodeState.SUCCESS;
    }
}