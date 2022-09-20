namespace Server.Mobiles;

public class DoAcquireNewCombatantInRange<T> : BTNode<T> where T : IAIState
{
    public override NodeState Evaluate(T treeState)
    {
        if (BaseBTAI<T>.AcquireFocusMob(treeState.Creature, treeState.Creature.RangePerception,
                treeState.Creature.FightMode, false, false, true))
        {
            treeState.Creature.DebugSay("My move is blocked, so I am going to attack {0}",
                treeState.Creature.FocusMob.Name);

            treeState.Creature.Combatant = treeState.Creature.FocusMob;
            treeState.Creature.FocusMob = null;

            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}