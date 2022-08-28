using System;

namespace Server.Items;

public class BaseBossChest : LockableContainer
{
    private LootResetTimer m_ResetTimer;

    [CommandProperty(AccessLevel.GameMaster)]
    public short MaxSpawnTime { get; set; } = 60;

    [CommandProperty(AccessLevel.GameMaster)]
    public short MinSpawnTime { get; set; } = 10;

    [CommandProperty(AccessLevel.GameMaster)]
    public override bool Locked
    {
        get => base.Locked;
        set
        {
            if (base.Locked != value)
            {
                base.Locked = value;

                if (!value)
                    StartResetTimer();
            }
        }
    }

    public override bool IsDecoContainer => false;

    public BaseBossChest(int itemID) : base(itemID)
    {
        Locked = true;
        Movable = false;

        SetLockLevel();
    }

    public BaseBossChest(Serial serial) : base(serial)
    {
    }

    public override void Serialize(IGenericWriter writer)
    {
        base.Serialize(writer);

        writer.Write(0);
        writer.Write(MinSpawnTime);
        writer.Write(MaxSpawnTime);
    }

    public override void Deserialize(IGenericReader reader)
    {
        base.Deserialize(reader);

        var version = reader.ReadInt();

        MinSpawnTime = reader.ReadShort();
        MaxSpawnTime = reader.ReadShort();

        if (!Locked)
            StartResetTimer();
    }

    protected virtual void SetLockLevel()
    {
        MaxLockLevel = 200;
        RequiredSkill = LockLevel = 200;
    }

    private void StartResetTimer()
    {
        if (m_ResetTimer == null)
            m_ResetTimer = new LootResetTimer(this);
        else
            m_ResetTimer.Delay = TimeSpan.FromMinutes(Utility.Random(MinSpawnTime, MaxSpawnTime));

        m_ResetTimer.Start();
    }

    public void ClearContents()
    {
        for (var i = Items.Count - 1; i >= 0; --i)
            if (i < Items.Count)
                Items[i].Delete();
    }

    public void Reset()
    {
        if (m_ResetTimer != null)
            if (m_ResetTimer.Running)
                m_ResetTimer.Stop();

        Locked = true;
        ClearContents();
    }

    private class LootResetTimer : Timer
    {
        private readonly BaseBossChest m_Chest;

        public LootResetTimer(BaseBossChest chest) : base(
            TimeSpan.FromMinutes(Utility.Random(chest.MinSpawnTime, chest.MaxSpawnTime)))
        {
            m_Chest = chest;
        }

        protected override void OnTick()
        {
            m_Chest.Reset();
        }
    }
}