using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Sequence : Node
{
    private List<Node> m_nodes = new List<Node>();

    public Sequence(List<Node> nodes)
    {
        m_nodes = nodes;
    }

    /* If any child node returns a failure, the entire node fails. Whence all  
     * nodes return a success, the node reports a success. */
    public override NodeState Evaluate()
    {
        bool anyChildRunning = false;

        foreach (Node node in m_nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.FAILURE:
                    _nodeState = NodeState.FAILURE;
                    return _nodeState;
                case NodeState.SUCCESS:
                    continue;
                case NodeState.RUNNING:
                    anyChildRunning = true;
                    continue;
                default:
                    _nodeState = NodeState.SUCCESS;
                    return _nodeState;
            }
        }
        _nodeState = anyChildRunning ? NodeState.RUNNING : NodeState.SUCCESS;
        return _nodeState;
    }
}
