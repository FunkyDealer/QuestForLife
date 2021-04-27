using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedStairs : MonoBehaviour
{
    public int numKeys;

    private void Start()
    {

    }

    public bool CheckKeys(List<int> keys)
    {
        if (keys.Count < numKeys)
        {
            return false;
        }
        else
        {

            return true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
