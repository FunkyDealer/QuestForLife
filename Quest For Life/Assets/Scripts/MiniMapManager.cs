using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapManager : MonoBehaviour
{
    Tile[,] map;

    [SerializeField]
    NavigationInterfaceManager interfaceManager;

    [SerializeField]
    GameObject miniMapGeneratorPrefab;

    public RectTransform agregator;

    public Dictionary<Tile, GameObject> tiles;

    [SerializeField]
    float TileSize;

    [SerializeField]
    RectTransform playerDisplay;

    void Awake()
    {
        Player.onPlayerMove += MoveWithPlayer;
        tiles = new Dictionary<Tile, GameObject>();
    }

    void OnDestroy()
    {
        Player.onPlayerMove -= MoveWithPlayer;
    }

    // Start is called before the first frame update
    void Start()
    {
        

        

        GenerateNewMiniMap();

        playerDisplay.sizeDelta = new Vector2(TileSize / 2, TileSize / 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateNewMiniMap()
    {
        tiles.Clear();
        int tilesNr = agregator.gameObject.transform.childCount;
        if (tilesNr > 0) {
            for (int i = 0; i < tilesNr; i++)
            {
                Destroy(agregator.gameObject.transform.GetChild(i).gameObject);
            }
        }

        map = interfaceManager.player.currentMap;
        agregator.sizeDelta = new Vector2(TileSize * map.GetLength(0), TileSize * map.GetLength(1));
        GameObject o = Instantiate(miniMapGeneratorPrefab, this.gameObject.transform);
        MiniMapGenerator m = o.GetComponent<MiniMapGenerator>();
        m.Init(map, this, interfaceManager.player, TileSize);

        ExploreMap(new Vector2(interfaceManager.player.currentTile.x, interfaceManager.player.currentTile.y));
    }


    void MoveWithPlayer(Vector2 newPos)
    {
        agregator.anchoredPosition = new Vector2(-newPos.y * TileSize + TileSize/2, newPos.x * TileSize + TileSize/2);

        ExploreMap(newPos);
    }

    public void ExploreMap(Vector2 newPos)
    {
        List<Vector2> UpdatedTiles = new List<Vector2>();

        int width = map.GetLength(0);
        int height = map.GetLength(1);
        Vector2 initialPos = new Vector2(newPos.x - 5, newPos.y - 5);
        for (int x = (int)initialPos.x; x < initialPos.x + 10; x++)
        {
            for (int y = (int)initialPos.y; y < (int)initialPos.y + 10; y++)
            {
                if (x >= 0 && y >= 0 && x < width && y < height && !map[x, y].explored)
                {
                    map[x, y].explored = true;
                    try
                    {
                        tiles[map[x, y]].SetActive(true);
                    } catch (KeyNotFoundException)
                    {

                    }
                }
            }
        }
    }

}
