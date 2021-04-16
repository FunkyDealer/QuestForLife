using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    public Dictionary<Tile, GameObject> FreeTiles;

    [HideInInspector]
    public Tile[,] map;

    [HideInInspector]
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void StartMap(MapGenerator DG, Vector2 spawn)
    {

    }


    public void AddFreeTile(int x, int y, GameObject obj)
    {
        FreeTiles.Add(map[x, y], obj);
    }

}
