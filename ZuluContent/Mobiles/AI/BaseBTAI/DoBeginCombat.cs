using System;

namespace Server.Mobiles;

public class DoBeginCombat<T> : BTNode<T> where T : IAIState
{
    public override NodeState Evaluate(T treeState)
    {
        treeState.Creature.Warmode = true;
        treeState.Creature.FocusMob = null;
        treeState.Creature.CurrentSpeed = treeState.Creature.ActiveSpeed;

        treeState.InCombat = true;

        treeState.Creature.DebugSay("Beginning combat");
        
        return NodeState.SUCCESS;
    }
}