using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopInterfaceManager : MonoBehaviour
{
    enum State
    {
        CHOOSING,
        BUYING,
        SELLING
    }
    State state;

    [HideInInspector]
    public Player player;
    [HideInInspector]
    public ShopManager ShopManager;
    public Shop shop => ShopManager.Shop;

    [SerializeField]
    GameObject ChoiceMenuObj;
    [SerializeField]
    GameObject BuyMenuObj;
    [SerializeField]
    GameObject SellMenuObj;

    [SerializeField]
    ShopBuyMenuController ShopBuyMenuController;

    [SerializeField]
    Text MoneyDisplay;

    bool locked;

    // Start is called before the first frame update
    void Start()
    {
        state = State.CHOOSING;
        BuyMenuObj.SetActive(false);
        SellMenuObj.SetActive(false);
        ChoiceMenuObj.SetActive(true);

        UpdateMoneyDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if (!locked && Input.GetButtonDown("CloseMenu"))
        {
            switch (state)
            {
                case State.CHOOSING:
                    CloseShopInterface();
                    break;
                case State.BUYING:
                    CloseBuyMenu();
                    break;
                case State.SELLING:
                    CloseSellMenu();
                    break;
            }            
        }

    }


    public void CloseShopInterface()
    {
        ShopManager.CloseShop(player, this.gameObject);
    }


    public void OpenBuyMenu()
    {
        CloseChoiceMenu();
        BuyMenuObj.SetActive(true);
        state = State.BUYING;
    }

    public void CloseBuyMenu()
    {
        BuyMenuObj.SetActive(false);
        OpenChoiceMenu();
    }

    public void OpenSellMenu()
    {
        CloseChoiceMenu();
        SellMenuObj.SetActive(true);
        state = State.SELLING;

    }

    public void CloseSellMenu()
    {
        SellMenuObj.SetActive(false);
        OpenChoiceMenu();        
    }

    public void OpenChoiceMenu()
    {
        ChoiceMenuObj.SetActive(true);
        state = State.CHOOSING;
    }

    public void CloseChoiceMenu()
    {
        ChoiceMenuObj.SetActive(false);
    }

    public void UpdateMoneyDisplay()
    {
        MoneyDisplay.text = player.currentGold.ToString() + "g";
    }

    public void LockInterface() => locked = true;
    public void unLockInterface() => locked = false;
}
