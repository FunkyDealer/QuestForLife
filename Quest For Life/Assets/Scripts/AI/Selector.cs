using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    public List<Node> m_nodes = new List<Node>();
    public Selector()
    {
       
    }

    public Selector(List<Node> nodes)
    {
        m_nodes = nodes;
    }

    /* If any of the children reports a success, the selector will 
     * immediately report a success upwards. If all children fail, 
     * it will report a failure instead.*/
    public override NodeState Evaluate()
    {
        foreach (Node node in m_nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.FAILURE:
                    continue;
                case NodeState.SUCCESS:
                    _nodeState = NodeState.SUCCESS;
                    return _nodeState;
                case NodeState.RUNNING:
                    _nodeState = NodeState.RUNNING;
                    return _nodeState;
                default:
                    continue;
            }
        }
        _nodeState = NodeState.FAILURE;
        return _nodeState;
    }
}
