﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopExit : PhysicalTile
{
    Animator animator;
    [HideInInspector]
    public ShopManager shopManager;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}