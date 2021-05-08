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

    [HideInInspector]
    protected int[] mapSeeds;
    public int[] MapSeeds => mapSeeds;
    protected int currentSeed = 0;
    public int CurrentSeed => currentSeed;

    

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

    public void increaseSeed()
    {
        currentSeed++;
        if (currentSeed == 254) currentSeed = 0;
    }


}
