using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericConfirmInterface : MonoBehaviour
{
    public Action action;
    public Action FinishAction; //unlock the screen Action

    [SerializeField]
    Text PromptText;

    [HideInInspector]
    public string text;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Init(Action action, Action finishAction, string text)
    {
        this.action = action;
        this.FinishAction = finishAction;
        this.text = text;

        PromptText.text = text;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("CloseMenu"))
        {
            DeclineButton();

        }
    }

    public void ConfirmButton()
    {
        action();

        Finish(); 
    }


    public void DeclineButton()
    {
        Finish();
    }


    void Finish()
    {
        FinishAction();
        Destroy(this.gameObject);
    }
}
