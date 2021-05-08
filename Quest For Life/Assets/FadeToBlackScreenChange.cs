using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeToBlackScreenChange : MonoBehaviour
{
    [HideInInspector]
    Action action;

    bool destroy;


    public void Init(Action action, bool destroy)
    {
        this.action = action;
        this.destroy = destroy;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Active()
    {
        action();
       if (destroy) Destroy(this.gameObject);
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
