using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericQuantityConfirmInterface : MonoBehaviour
{
    [HideInInspector]
    public Action<int> action;
    [HideInInspector]
    public Action FinishAction; //unlock the screen Action

    [SerializeField]
    Text PromptText;

    [HideInInspector]
    public string text;

    [SerializeField]
    Text ammountText;
    [SerializeField]
    GameObject moreQuantityButton;
    [SerializeField]
    GameObject lessQuantityButton;

    int ammount = 1;
    int maxAmmount;
    int minAmmount = 1;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Init(Action<int> action, Action finishAction, string text, int maxAmmount)
    {
        this.action = action;
        this.FinishAction = finishAction;
        this.text = text;
        this.maxAmmount = maxAmmount;

        minAmmount = 1;
        ammount = 1;

        PromptText.text = text;
        updateAmmountText();
        setButtons();
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
        action(ammount);

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

    public void addQuantity()
    {
        ammount++;
        if (ammount == maxAmmount) moreQuantityButton.SetActive(false);
        if (ammount > minAmmount) lessQuantityButton.SetActive(true);
        updateAmmountText();
    }

    public void removeQuantity()
    {
        ammount--;
        if (ammount < maxAmmount) moreQuantityButton.SetActive(true);
        if (ammount == minAmmount) lessQuantityButton.SetActive(false);
        updateAmmountText();
    }

    void updateAmmountText()
    {
        ammountText.text = ammount.ToString();
    }

    void setButtons()
    {
        lessQuantityButton.SetActive(false);
        if (minAmmount == maxAmmount) moreQuantityButton.SetActive(false);
        else if (maxAmmount > minAmmount) moreQuantityButton.SetActive(true);



    }
}
