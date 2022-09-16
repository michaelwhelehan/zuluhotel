using System.Collections.Generic;

namespace Server.Mobiles;

public abstract class BTTree
{
    private BTNode m_Root;

    private readonly Dictionary<string, object> m_DataContext = new();

    public void Start()
    {
        m_Root = SetupTree();
    }

    public NodeState Update()
    {
        return m_Root.Evaluate();
    }

    protected abstract BTNode SetupTree();

    public void SetData(string key, object value)
    {
        m_DataContext[key] = value;
    }

    public object GetData(string key)
    {
        m_DataContext.TryGetValue(key, out var value);
        return value;
    }

    public bool ClearData(string key)
    {
        if (!m_DataContext.ContainsKey(key)) return false;
        m_DataContext.Remove(key);
        return true;
    }
}