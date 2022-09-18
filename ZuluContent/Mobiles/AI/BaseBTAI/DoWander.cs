namespace Server.Mobiles;

public class DoWander : BTNode
{
    private readonly BaseCreature m_Mobile;
    private readonly BTTree m_BtTree;
    private readonly int m_ChanceToNotMove;
    private readonly int m_ChanceToDir;
    private readonly int m_Steps;
    
    public DoWander(BaseCreature mobile, BTTree tree, int chanceToNotMove, int chanceToDir, int steps)
    {
        m_Mobile = mobile;
        m_BtTree = tree;
        m_ChanceToNotMove = chanceToNotMove;
        m_ChanceToDir = chanceToDir;
        m_Steps = steps;
    }

    private bool _DoMove(Direction d)
    {
        var nextMove = (long) (m_BtTree.GetData("nextMove") ?? Core.TickCount);
        
        var res = BaseBTAI.DoMoveImpl(m_Mobile, d, ref nextMove);
        
        m_BtTree.SetData("nextMove", nextMove);

        return res is MoveResult.Success or MoveResult.SuccessAutoTurn;
    }
    
    private void _WalkRandom(int chanceToNotMove, int chanceToDir, int steps)
    {
        if (m_Mobile.Deleted || m_Mobile.DisallowAllMoves)
            return;

        for (int i = 0; i < steps; i++)
        {
            if (Utility.Random(8 * chanceToNotMove) <= 8)
            {
                int iRndMove = Utility.Random(0, 8 + 9 * chanceToDir);

                switch (iRndMove)
                {
                    case 0:
                        _DoMove(Direction.Up);
                        break;
                    case 1:
                        _DoMove(Direction.North);
                        break;
                    case 2:
                        _DoMove(Direction.Left);
                        break;
                    case 3:
                        _DoMove(Direction.West);
                        break;
                    case 5:
                        _DoMove(Direction.Down);
                        break;
                    case 6:
                        _DoMove(Direction.South);
                        break;
                    case 7:
                        _DoMove(Direction.Right);
                        break;
                    case 8:
                        _DoMove(Direction.East);
                        break;
                    default:
                        _DoMove(m_Mobile.Direction);
                        break;
                }
            }
        }
    }

    public override NodeState Evaluate()
    {
        if (m_Mobile.Deleted || m_Mobile.DisallowAllMoves || m_Mobile.Hidden)
        {
            State = NodeState.FAILURE;
            return State;
        }
        
        m_Mobile.DebugSay("I am wandering");
            
        if (m_Mobile.Home == Point3D.Zero)
        {
            if (m_Mobile.Spawner != null)
            {
                var region = m_Mobile.Spawner.Region;

                if (m_Mobile.Region.AcceptsSpawnsFrom(region))
                {
                    m_Mobile.WalkRegion = region;
                    _WalkRandom(m_ChanceToNotMove, m_ChanceToDir, m_Steps);
                    m_Mobile.WalkRegion = null;
                }
                else
                {
                    if (region.GoLocation != Point3D.Zero && Utility.Random(10) > 5)
                    {
                        _DoMove(m_Mobile.GetDirectionTo(region.GoLocation));
                    }
                    else
                    {
                        _WalkRandom(m_ChanceToNotMove, m_ChanceToDir, 1);
                    }
                }
            }
            else
            {
                _WalkRandom(m_ChanceToNotMove, m_ChanceToDir, m_Steps);
            }
        }
        else
        {
            for (int i = 0; i < m_Steps; i++)
            {
                if (m_Mobile.RangeHome != 0)
                {
                    int iCurrDist = (int) m_Mobile.GetDistanceToSqrt(m_Mobile.Home);

                    if (iCurrDist < m_Mobile.RangeHome * 2 / 3)
                    {
                        _WalkRandom(m_ChanceToNotMove, m_ChanceToDir, 1);
                    }
                    else if (iCurrDist > m_Mobile.RangeHome)
                    {
                        _DoMove(m_Mobile.GetDirectionTo(m_Mobile.Home));
                    }
                    else
                    {
                        if (Utility.Random(10) > 5)
                        {
                            _DoMove(m_Mobile.GetDirectionTo(m_Mobile.Home));
                        }
                        else
                        {
                            _WalkRandom(m_ChanceToNotMove, m_ChanceToDir, 1);
                        }
                    }
                }
                else
                {
                    if (m_Mobile.Location != m_Mobile.Home)
                    {
                        _DoMove(m_Mobile.GetDirectionTo(m_Mobile.Home));
                    }
                }
            }
        }

        State = NodeState.RUNNING;
        return State;
    }
}