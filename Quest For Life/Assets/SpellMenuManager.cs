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
    RectTransform Content;

    [SerializeField]
    GameObject nextObject;

    [SerializeField]
    GameObject previousObject;

    int page;
    int pageTotal;

    Dictionary<int, GameObject> buttons;

    void Awake()
    {
        page = 1;
        pageTotal = 0;
        previousObject.SetActive(false);
        buttons = new Dictionary<int, GameObject>();
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
        int buttonNum = 1;
        foreach (var spell in manager.player.getKnownSpells())
        {
            GameObject o = Instantiate(buttonPrefab, Content.gameObject.transform);
            BattleSpellButton b = o.GetComponent<BattleSpellButton>();
            b.assignedSpell = spell;
            b.battleManager = manager;

            buttons.Add(buttonNum, o);
            buttonNum++;
            if (buttonNum > (pageTotal * 6)) pageTotal++;
        }
      
    }

    public void NextPage()
    {
        Content.anchoredPosition -= new Vector2(273, 0);
        page++;
        if (page > 1) previousObject.SetActive(true);
        if (page == 3) nextObject.SetActive(false);
    }

    public void PreviousPage()
    {
        Content.anchoredPosition += new Vector2(273, 0);
        page--;
        if (page < 3) nextObject.SetActive(true);
        if (page == 1) previousObject.SetActive(false);
    }

}
