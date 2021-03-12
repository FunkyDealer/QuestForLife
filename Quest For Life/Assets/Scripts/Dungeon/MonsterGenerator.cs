using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator 
{

    public Global.DungeonMonsterInfo DungeonMonsterInfo;
    Dictionary<int, GameObject> MonsterPrefabs;



    public MonsterGenerator(Dictionary<int, GameObject> MonsterPrefabs)
    {
        MonsterPrefabs = new Dictionary<int, GameObject>();
        this.MonsterPrefabs = MonsterPrefabs;
    }


    void Awake()
    {
        

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
