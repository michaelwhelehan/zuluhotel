using System.Collections.Generic;

namespace Server.Mobiles;

public class MeleeBT<T> : BaseSingleton<MeleeBT<T>>, IBTTree<T> where T : IAIState
{
    public BTNode<T> Root { get; }

    private MeleeBT()
    {
        Root = SetupTree();
    }
    
    public static BTNode<T> DoCombat()
    {
        return new BTSequence<T>(new List<BTNode<T>>
        {
            new BTSelector<T>(new List<BTNode<T>>
            {
                new CheckCombatantExists<T>(),
                new CheckCombat<T>(),
            }),
            new DoEndCombat<T>(),
            new BTSelector<T>(new List<BTNode<T>>
            {
                new CheckCombatantExists<T>(),
                new BTInverter<T>(new BTSucceeder<T>(new DoBeginGuarding<T>()))
            }),
            new BTSelector<T>(new List<BTNode<T>>
            {
                new CheckCombatantInRange<T>(),
                new DoAcquireNewCombatantInRange<T>(),
                new CheckCombatantInRange<T>(3),
                new BTInverter<T>(new BTSucceeder<T>(new DoRemoveCombatant<T>()))
            }),
            new DoBeginCombat<T>(),
            new CheckMoveTo<T>(),
            new BTSelector<T>(new List<BTNode<T>>
            {
                new BTSequence<T>(new List<BTNode<T>>
                {
                    new CheckCombatantInAttackRange<T>(),
                    new DoClearPath<T>()
                }),
                new BTSequence<T>(new List<BTNode<T>>
                {
                    new CheckPathExists<T>(),
                    new DoFollowPath<T>(),
                    new DoClearPath<T>()
                }),
                new DoMoveToTarget<T>(MoveToTargetType.Combatant, true),
                new BTSequence<T>(new List<BTNode<T>>
                {
                    new DoCreatePath<T>(),
                    new DoFollowPath<T>(),
                    new DoClearPath<T>()
                }),
                new DoAcquireNewCombatantInRange<T>(),
                new BTInverter<T>(new CheckTargetTooFarAway<T>()),
                new DoBeginGuarding<T>()
            }),
        });
    }
    
    public static BTNode<T> DoGuard()
    {
        return new BTSequence<T>(new List<BTNode<T>>
        {
            new BTSelector<T>(new List<BTNode<T>>
            {
                new CheckGuarding<T>(),
                new BTInverter<T>(new BTSucceeder<T>(new DoEndGuarding<T>()))
            }),
            new BTSucceeder<T>(new DoAcquireNewCombatantInRange<T>()),
            new DoProcessGuarding<T>(),
            new BTInverter<T>(new BTSucceeder<T>(new DoEndGuarding<T>()))
        });
    }
    
    public static BTNode<T> DoAttack()
    {
        return new BTSequence<T>(new List<BTNode<T>>
        {
            new CheckControlOrder<T>(OrderType.Attack),
            new DoBeginAttack<T>(),
            new BTSelector<T>(new List<BTNode<T>>
            {
                DoCombat(),
                DoLastOrder()
            }),
        });
    }

    public static BTNode<T> DoLastOrder()
    {
        return new BTInverter<T>(new BTSucceeder<T>(new BTSequence<T>(new List<BTNode<T>>
        {
            new CheckLastOrderFollowing<T>(),
            new DoChangeLastOrder<T>()
        })));
    }
    
    public static BTNode<T> DoFollow()
    {
        return new BTSequence<T>(new List<BTNode<T>>
        {
            new CheckControlOrder<T>(OrderType.Follow),
            new CheckMoveTo<T>(MoveToTargetType.ControlTarget),
            new DoBeginFollow<T>(),
            new BTSelector<T>(new List<BTNode<T>>
            {
                new CheckTargetTooFarAway<T>(MoveToTargetType.ControlTarget),
                new BTSequence<T>(new List<BTNode<T>>
                {
                    new CheckTargetTooClose<T>(MoveToTargetType.ControlTarget, 1),
                    new DoClearPath<T>(),
                }),
                new BTSequence<T>(new List<BTNode<T>>
                {
                    new CheckPathExists<T>(),
                    new DoFollowPath<T>(),
                    new DoClearPath<T>()
                }),
                new DoMoveToTarget<T>(MoveToTargetType.ControlTarget, true),
                new BTSequence<T>(new List<BTNode<T>>
                {
                    new DoCreatePath<T>(MoveToTargetType.ControlTarget),
                    new DoFollowPath<T>(),
                    new DoClearPath<T>()
                }),
            }),
        });
    }
    
    public static BTNode<T> DoCome()
    {
        return new BTSequence<T>(new List<BTNode<T>>
        {
            new CheckControlOrder<T>(OrderType.Come),
            new CheckMoveTo<T>(MoveToTargetType.ControlMaster),
            new BTInverter<T>(new CheckTargetTooFarAway<T>(MoveToTargetType.ControlMaster)),
            new DoBeginCome<T>(),
            new BTSelector<T>(new List<BTNode<T>>
            {
                new BTSequence<T>(new List<BTNode<T>>
                {
                    new CheckTargetTooClose<T>(MoveToTargetType.ControlMaster, 1),
                    new DoClearPath<T>(),
                }),
                new BTSequence<T>(new List<BTNode<T>>
                {
                    new CheckPathExists<T>(),
                    new DoFollowPath<T>(),
                    new DoClearPath<T>()
                }),
                new DoMoveToTarget<T>(MoveToTargetType.ControlMaster, true),
                new BTSequence<T>(new List<BTNode<T>>
                {
                    new DoCreatePath<T>(MoveToTargetType.ControlMaster),
                    new DoFollowPath<T>(),
                    new DoClearPath<T>()
                }),
            }),
        });
    }
    
    public static BTNode<T> DoStop()
    {
        return new BTSequence<T>(new List<BTNode<T>>
        {
            new CheckControlOrder<T>(OrderType.Stop),
            new BTInverter<T>(new BTSucceeder<T>(new DoBeginStop<T>()))
        });
    }
    
    public static BTNode<T> DoStay()
    {
        return new BTSequence<T>(new List<BTNode<T>>
        {
            new CheckControlOrder<T>(OrderType.Stay),
            new DoBeginStay<T>()
        });
    }
    
    public static BTNode<T> DoObey()
    {
        return new BTSelector<T>(new List<BTNode<T>>
        {
            DoAttack(),
            DoFollow(),
            DoCome(),
            DoStop(),
            DoStay(),
            new DoWander<T>(3, 2, 1)
        });
    }

    public static BTNode<T> DoThink()
    {
        return new BTSelector<T>(new List<BTNode<T>>
        {
            DoCombat(),
            DoGuard(),
            new DoAcquireNewCombatantInRange<T>(),
            new BTInverter<T>(new CheckIdle<T>()),
            new DoWander<T>(2, 2, 1)
        });
    }

    private BTNode<T> SetupTree()
    {
        var root = new BTSequence<T>(new List<BTNode<T>>
        {
            new BTInverter<T>(new CheckIsDeleted<T>()),
            new BTInverter<T>(new CheckMapIsNullOrInternal<T>()),
            new BTInverter<T>(new CheckMapSectorInactive<T>()),
            new BTSelector<T>(new List<BTNode<T>>
            {
                new BTSequence<T>(new List<BTNode<T>>
                {
                    new CheckBardPacified<T>(),
                    new DoBardPacified<T>()
                }),
                new BTSequence<T>(new List<BTNode<T>>
                {
                    new CheckBardProvoked<T>(),
                    new BTSucceeder<T>(DoCombat()),
                    new DoBardProvoked<T>()
                }),
                new BTSequence<T>(new List<BTNode<T>>
                {
                    new CheckControlled<T>(),
                    DoObey()
                }),
                new BTSequence<T>(new List<BTNode<T>>
                {
                    new BTInverter<T>(new CheckControlled<T>()),
                    DoThink()
                })
            })
        });

        return root;
    }
}