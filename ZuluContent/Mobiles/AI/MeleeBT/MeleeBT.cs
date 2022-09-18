using System.Collections.Generic;

namespace Server.Mobiles;

public class MeleeBT : BaseBT
{
    public MeleeBT(BaseCreature mobile) : base(mobile)
    {
    }
    
    public static BTNode DoCombat(BaseCreature mobile, BTTree tree)
    {
        return new BTSequence(new List<BTNode>
        {
            new BTSelector(new List<BTNode>
            {
                new CheckCombatantExists(mobile),
                new CheckCombat(tree),
            }),
            new DoEndCombat(tree),
            new BTSelector(new List<BTNode>
            {
                new CheckCombatantExists(mobile),
                new BTInverter(new BTSucceeder(new DoBeginGuarding(mobile, tree)))
            }),
            new BTSelector(new List<BTNode>
            {
                new CheckCombatantInRange(mobile),
                new DoAcquireNewCombatantInRange(mobile),
                new CheckCombatantInRange(mobile, 3),
                new BTInverter(new BTSucceeder(new DoRemoveCombatant(mobile)))
            }),
            new DoBeginCombat(mobile, tree),
            new CheckMoveTo(mobile),
            new BTSelector(new List<BTNode>
            {
                new BTSequence(new List<BTNode>
                {
                    new CheckCombatantInAttackRange(mobile),
                    new DoClearPath(tree)
                }),
                new BTSequence(new List<BTNode>
                {
                    new CheckPathExists(mobile, tree),
                    new DoFollowPath(tree, true),
                    new DoClearPath(tree)
                }),
                new DoMoveToCombatant(mobile, tree, true),
                new BTSequence(new List<BTNode>
                {
                    new DoCreatePath(mobile, tree),
                    new DoFollowPath(tree, true),
                    new DoClearPath(tree)
                }),
                new DoAcquireNewCombatantInRange(mobile),
                new BTInverter(new CheckCombatantTooFarAway(mobile)),
                new DoBeginGuarding(mobile, tree)
            }),
        });
    }
    
    public static BTNode DoGuard(BaseCreature mobile, BTTree tree)
    {
        return new BTSequence(new List<BTNode>
        {
            new BTSelector(new List<BTNode>
            {
                new CheckGuarding(tree),
                new BTInverter(new BTSucceeder(new DoEndGuarding(tree)))
            }),
            new BTSucceeder(new DoAcquireNewCombatantInRange(mobile)),
            new DoProcessGuarding(tree),
            new BTInverter(new BTSucceeder(new DoEndGuarding(tree)))
        });
    }

    public static BTNode DoThink(BaseCreature mobile, BTTree tree)
    {
        return new BTSelector(new List<BTNode>
        {
            DoGuard(mobile, tree),
            DoCombat(mobile, tree),
            new DoAcquireNewCombatantInRange(mobile),
            new BTInverter(new CheckIdle(mobile)),
            new DoWander(mobile, tree, 2, 2, 1)
        });
    }

    protected override BTNode SetupTree()
    {
        var root = new BTSequence(new List<BTNode>
        {
            new BTInverter(new CheckIsDeleted(Mobile)),
            new BTInverter(new CheckMapIsNullOrInternal(Mobile)),
            new BTInverter(new CheckMapSectorInactive(Mobile)),
            new BTSelector(new List<BTNode>
            {
                new BTSequence(new List<BTNode>
                {
                    new CheckBardPacified(Mobile),
                    new DoBardPacified(Mobile)
                }),
                new BTSequence(new List<BTNode>
                {
                    new CheckBardProvoked(Mobile),
                    new BTSucceeder(DoCombat(Mobile, this)),
                    new DoBardProvoked(Mobile)
                }),
                new BTSequence(new List<BTNode>
                {
                    new BTInverter(new CheckControlled(Mobile)),
                    DoThink(Mobile, this)
                }),
            })
        });

        return root;
    }
}