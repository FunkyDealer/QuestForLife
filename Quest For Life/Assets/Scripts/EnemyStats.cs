using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    Enemy us;
    Text ui;
    void Start()
    {
        us = FindObjectOfType<Enemy>();
        ui = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        ui.text = us.name + "\n" + "hp: " + us.currentHealth;
    }
}
