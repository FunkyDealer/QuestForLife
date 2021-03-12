using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TesteExcel : MonoBehaviour
{
    public TextAsset Excel;

    [Serializable]
    public class Dados
    {
        public string nome;
        public int hp;
        public int poder;
    }

    public Dados[] dados;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Começo");
        ReadExcel();
    }


    void ReadExcel()
    {
        string[] data = Excel.text.Split(new string[] { ";", "\n" }, StringSplitOptions.None);
        //   Debug.Log(Excel.text);
        //   Debug.Log(data.Length);
        int size = data.Length / 3 - 1;
        dados = new Dados[size];
        //    Debug.Log(size);
        for (int i = 0; i < size; i++)
        {
            Debug.Log(i);
            dados[i] = new Dados();
            dados[i].nome = data[3 * (i + 1)];
            dados[i].hp = int.Parse(data[3 * (i + 1) + 1]);
            dados[i].poder = int.Parse(data[3 * (i + 1) + 2]);
        }
        Debug.Log("Fim");
    }
}
