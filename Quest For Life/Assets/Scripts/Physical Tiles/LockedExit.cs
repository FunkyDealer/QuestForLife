using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedExit : MonoBehaviour
{
    public int numKeys;

    private void Start()
    {

    }

    public bool VerificarChaves(List<int> keys)
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
}
