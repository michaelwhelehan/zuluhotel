using System.Collections.Generic;

namespace Server.Mobiles;

public class MeleeBT : BTTree
{
    private BaseCreature m_Mobile;
    
    public MeleeBT(BaseCreature mobile)
    {
        m_Mobile = mobile;
    }
    
    public static BTNode DoCombat(BaseCreature mobile, BTTree tree)
    {
        return new BTSequence(new List<BTNode>
        {
            new CheckCombatantExists(mobile),
            new BTSelector(new List<BTNode>
            {
                new CheckCombatantInRange(mobile),
                new DoAcquireNewCombatantInRange(mobile),
                new CheckCombatantInRange(mobile, 3),
                new BTInverter(new BTSucceeder(new DoRemoveCombatant(mobile)))
            }),
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
            new DoAcquireNewCombatantInRange(mobile),
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
            new BTInverter(new CheckIsDeleted(m_Mobile)),
            new BTInverter(new CheckMapIsNullOrInternal(m_Mobile)),
            new BTInverter(new CheckMapSectorInactive(m_Mobile)),
            new BTSelector(new List<BTNode>
            {
                new BTSequence(new List<BTNode>
                {
                    new CheckBardPacified(m_Mobile),
                    new DoBardPacified(m_Mobile)
                }),
                new BTSequence(new List<BTNode>
                {
                    new CheckBardProvoked(m_Mobile),
                    new BTSucceeder(DoCombat(m_Mobile, this)),
                    new DoBardProvoked(m_Mobile)
                }),
                new BTSequence(new List<BTNode>
                {
                    new BTInverter(new CheckControlled(m_Mobile)),
                    DoThink(m_Mobile, this)
                }),
            })
        });

        return root;
    }
}