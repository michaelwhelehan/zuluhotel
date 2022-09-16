namespace Server.Mobiles;

public class CheckMapSectorInactive : BTNode
{
    private readonly BaseCreature m_Mobile;
    
    public CheckMapSectorInactive(BaseCreature mobile)
    {
        m_Mobile = mobile;
    }

    public override NodeState Evaluate()
    {
        if (m_Mobile.PlayerRangeSensitive)
        {
            var sect = m_Mobile.Map.GetSector(m_Mobile.Location);

            if (!sect.Active)
            {
                State = NodeState.SUCCESS;
                return State;
            }
        }

        State = NodeState.FAILURE;
        return State;
    }
}