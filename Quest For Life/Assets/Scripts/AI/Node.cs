using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node 
{
    public enum NodeState { FAILURE,SUCCESS,RUNNING}
    
    protected NodeState _nodeState;
    public NodeState nodestate { get { return _nodeState; } }

    public abstract NodeState Evaluate();
}
