using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePlayerStatsManager : MonoBehaviour
{
    [SerializeField]
    GameObject Health;
    [SerializeField]
    GameObject Mana;
    [SerializeField]
    Text Name;

    [SerializeField]
    HudManager manager;

    [SerializeField]
    Text HealthText;
    [SerializeField]
    Text ManaText;

    RectTransform healthBar;
    RectTransform manaBar;
    float healthBarMaxSize;
    float manaBarMaxSize;
    float minBarSize = 0;    

    Player player;



    void Awake()
    {
        healthBar = Health.GetComponent<RectTransform>();
        manaBar = Mana.GetComponent<RectTransform>();

        //Bar Sizes
        healthBarMaxSize = healthBar.sizeDelta.x;
        manaBarMaxSize = manaBar.sizeDelta.x;
        
        Player.onHealthUpdate += UpdateHealth;
        Player.onManaUpdate += UpdateMana;
        
    }

    void OnDestroy()
    {
        Player.onHealthUpdate -= UpdateHealth;
        Player.onManaUpdate -= UpdateMana;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = manager.player;
        UpdateHealth(player.currentHealth, player.maxHealth);
        UpdateMana(player.currentMana, player.maxMana);
        updatePlayerText();
    }

    void updatePlayerText()
    {

        Name.text = $"The Mage Lv.{player.Level}";

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UpdateHealth(int currentHealth, int maxH)
    {
        HealthText.text = currentHealth.ToString();

        //maxHealth - maxBarSize
        //CurrentHealth - x

        int x = (int)((healthBarMaxSize * currentHealth) / maxH);
        healthBar.sizeDelta = new Vector2(x, healthBar.sizeDelta.y);

    }

    public void UpdateMana(int currentMana, int maxM)
    {
        ManaText.text = currentMana.ToString();

        //maxMana - maxBarSize
        //CurrentMana - x

        int x = (int)((manaBarMaxSize * currentMana) / maxM);
        manaBar.sizeDelta = new Vector2(x, manaBar.sizeDelta.y);

    }



}
