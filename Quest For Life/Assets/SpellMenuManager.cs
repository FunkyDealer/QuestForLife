using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellMenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject buttonPrefab;

    [SerializeField]
    BattleInterFaceManager manager;

    [SerializeField]
    RectTransform FireContent;
    [SerializeField]
    RectTransform WaterContent;
    [SerializeField]
    RectTransform ThunderContent;
    [SerializeField]
    RectTransform LightContent;

    Dictionary<int, GameObject> FireButtons;
    Dictionary<int, GameObject> WaterButtons;
    Dictionary<int, GameObject> ThunderButtons;
    Dictionary<int, GameObject> LightButtons;

    [SerializeField]
    List<Button> buttons;

    List<GameObject> menuContent;


    void Awake()
    {
        FireButtons = new Dictionary<int, GameObject>();
        WaterButtons = new Dictionary<int, GameObject>();
        ThunderButtons = new Dictionary<int, GameObject>();
        LightButtons = new Dictionary<int, GameObject>();

        menuContent = new List<GameObject>();

        menuContent.Add(FireContent.gameObject);
        menuContent.Add(WaterContent.gameObject);
        menuContent.Add(ThunderContent.gameObject);
        menuContent.Add(LightContent.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        PopulateButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopulateButtons()
    {
        int fireButtonNum = 0;
        int waterButtonNum = 0;
        int thunderButtonNum = 0;
        int lightButtonNum = 0;
        

        foreach (var spell in manager.player.getKnownSpells())
        {
            switch (spell.type)
            {
                case Global.Type.FIRE:                   
                    fireButtonNum++;
                    FireButtons.Add(fireButtonNum, createButton(spell, FireContent, fireButtonNum));
                    break;
                case Global.Type.WATER:
                    waterButtonNum++;
                    WaterButtons.Add(waterButtonNum, createButton(spell, WaterContent, waterButtonNum));
                    break;
                case Global.Type.THUNDER:
                    thunderButtonNum++;
                    ThunderButtons.Add(thunderButtonNum, createButton(spell, ThunderContent, thunderButtonNum));
                    break;
                case Global.Type.LIGHT:
                    lightButtonNum++;
                    LightButtons.Add(lightButtonNum, createButton(spell, LightContent, lightButtonNum));
                    break;
                default:
                    break;
            }           
        }      
    }

    GameObject createButton(Global.Spell spell, RectTransform parent, int ButtonNum)
    {
        parent.sizeDelta = new Vector2(parent.sizeDelta.x, 30 * (ButtonNum));

        GameObject o = Instantiate(buttonPrefab, parent.gameObject.transform);
        BattleSpellButton b = o.GetComponent<BattleSpellButton>();

        b.assignedSpell = spell;
        b.battleManager = manager;
        if (spell.type == Global.Type.LIGHT) b.target = true;
        else b.target = false;

        return o;
    }


    public void FireButtonClick()
    {
        if (manager.canAct && !manager.selectingTarget)
        {
            if (FireContent.gameObject.activeInHierarchy) FireContent.gameObject.SetActive(false);
            else
            {
                disableAllContentMenus();

                FireContent.gameObject.SetActive(true);
            }
        }
    }

    public void ThunderButtonClick()
    {
        if (manager.canAct && !manager.selectingTarget)
        {
            if (ThunderContent.gameObject.activeInHierarchy) ThunderContent.gameObject.SetActive(false);
            else
            {
                disableAllContentMenus();

                ThunderContent.gameObject.SetActive(true);
            }
        }
    }

    public void WaterButtonClick()
    {
        if (manager.canAct && !manager.selectingTarget)
        {
            if (WaterContent.gameObject.activeInHierarchy) WaterContent.gameObject.SetActive(false);
            else
            {
                disableAllContentMenus();

                WaterContent.gameObject.SetActive(true);
            }
        }
    }

    public void LightButtonClick()
    {
        if (manager.canAct && !manager.selectingTarget)
        {
            if (LightContent.gameObject.activeInHierarchy) LightContent.gameObject.SetActive(false);
            else
            {
                disableAllContentMenus();

                LightContent.gameObject.SetActive(true);
            }
        }
    }

    public void disableAllContentMenus()
    {
        foreach (var m in menuContent)
        {
            m.SetActive(false);
        }
    }

}
