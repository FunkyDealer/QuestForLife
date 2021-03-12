using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{

    public Entity attacker;
    public Entity defender;


    public Text info_Display;
    [SerializeField]
    Text info_Hp, info_Mp;

    public int round = 0;
    bool OurTurn = true;

    // Start is called before the first frame update
    void Start()
    {
        attacker = FindObjectOfType<Player>();
        defender = FindObjectOfType<Enemy>();
        round = Random.Range(1, 3);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Round();

        }
        if (round % 2 == 0)
        {
            OurTurn = true;
            info_Display.text = "\n Our Turn";
        }
        else
        {
            OurTurn = false;
        }

        info_Hp.text = "HP: " + attacker.Cur_Healt();
        info_Mp.text = "MP: " + attacker.Cur_Mana();

        if (attacker.Cur_Healt() <= 0)
        {
            info_Display.text += "\n YOU LOSE";

        }
        else if (defender.Cur_Healt() <= 0)
        {
            info_Display.text += "\n YOU WIN";
        }

    }


    public void Round()
    {
        round++;
        Debug.Log("NEW Round");
    }
}
