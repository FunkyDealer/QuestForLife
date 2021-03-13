using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    [HideInInspector]
    public Dictionary<int, Global.DungeonMonsterInfo> Monsters;
    [HideInInspector]
    public Dictionary<int, Global.Spell> Spells;

    private static DataBase _instance;
    public static DataBase inst { get { return _instance; } }

    [HideInInspector]
    public Dictionary<int, GameObject> MonsterPrefabs;
    [SerializeField]
    List<GameObject> MPrefabs;

    [SerializeField]
    TextAsset MonsterInfoFile;
    [SerializeField]
    TextAsset SpellInfoFile;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        MonsterPrefabs = new Dictionary<int, GameObject>();

        int i = 1;
        foreach (var o in MPrefabs)
        {
            MonsterPrefabs.Add(i, o);
            i++;
        }

    }


    // Start is called before the first frame update
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //void ReadExcel()
    //{
    //public TextAsset Excel;
    //
    //    string[] data = Excel.text.Split(new string[] { ";", "\n" }, StringSplitOptions.None);
    //    //   Debug.Log(Excel.text);
    //    //   Debug.Log(data.Length);
    //    int size = data.Length / 3 - 1;
    //    dados = new Dados[size];
    //    //    Debug.Log(size);
    //    for (int i = 0; i < size; i++)
    //    {
    //        Debug.Log(i);
    //        dados[i] = new Dados();
    //        dados[i].nome = data[3 * (i + 1)];
    //        dados[i].hp = int.Parse(data[3 * (i + 1) + 1]);
    //        dados[i].poder = int.Parse(data[3 * (i + 1) + 2]);
    //    }
    //    Debug.Log("Fim");

    //    text.text = $" {dados[0].nome}; {dados[0].hp}; {dados[0].poder}; /n {dados[1].nome};  {dados[1].hp}; {dados[1].poder}  ";
    //}
}
