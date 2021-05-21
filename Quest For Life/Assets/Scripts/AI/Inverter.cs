using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : Node
{
    private Node m_node;

    public Node node
    {
        get { return m_node; }
    }

    public Inverter(Node node)
    {
        m_node = node;
    }

    /* Reports a success if the child fails and 
     * a failure if the child succeeds. Running will report 
     * as running */
    public override NodeState Evaluate()
    {
        switch (m_node.Evaluate())
        {
            case NodeState.FAILURE:
                _nodeState = NodeState.SUCCESS;
                return _nodeState;
            case NodeState.SUCCESS:
                _nodeState = NodeState.FAILURE;
                return _nodeState;
            case NodeState.RUNNING:
                _nodeState = NodeState.RUNNING;
                return _nodeState;
        }
        _nodeState = NodeState.SUCCESS;
        return _nodeState;
    }
}
