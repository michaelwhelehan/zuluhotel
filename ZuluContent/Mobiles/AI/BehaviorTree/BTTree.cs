using System;
using System.Collections.Generic;

namespace Server.Mobiles;

public abstract class BTTree
{
    private BTNode m_Root;

    private readonly Dictionary<string, object> m_DataContext = new();

    private readonly Queue<BTNode> m_CurrentProcessingNodes = new();

    public void Init()
    {
        m_Root = SetupTree();
    }

    public NodeState Update()
    {
        /*try
        {
            var currentProcessingNode = PeekProcessingNode();
            if (currentProcessingNode != null)
            {
                var state = currentProcessingNode.Evaluate();
                if (state != NodeState.RUNNING)
                {
                    DequeueProcessingNode();
                }

                return state;
            }
            
            return m_Root.Evaluate();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NodeState.FAILURE;
        }*/
        
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

    public void QueueProcessingNode(BTNode node)
    {
        m_CurrentProcessingNodes.Enqueue(node);
    }
    
    public BTNode DequeueProcessingNode()
    {
        return m_CurrentProcessingNodes.Dequeue();
    }
    
    public BTNode PeekProcessingNode()
    {
        m_CurrentProcessingNodes.TryPeek(out var value);

        return value;
    }
}