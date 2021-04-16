using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapGenerator : MonoBehaviour
{
    Tile[,] map;

    MiniMapManager manager;

    [SerializeField]
    GameObject TilePrefab;

    RectTransform agregator;

    Player player;

    public void Init(Tile[,] map, MiniMapManager manager, Player player)
    {
        this.map = map;
        this.manager = manager;
        this.agregator = manager.agregator;
        this.player = player;
        GenerateMap();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void GenerateMap()
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                Vector2 position = new Vector2(x * 10, y * 10);
                switch (map[x,y].type)
                {
                    case Tile.Type.room:
                        InstantiateTile(position);
                        break;
                    case Tile.Type.hall:
                        InstantiateTile(position);
                        break;
                    case Tile.Type.wall:

                        break;
                    case Tile.Type.filling:

                        break;
                    case Tile.Type.none:
                        break;
                    default:
                        break;
                }


            }
        }

        Finish();
    }

    void InstantiateTile(Vector2 pos)
    {
        GameObject o = Instantiate(TilePrefab, agregator.gameObject.transform);
        RectTransform r = o.GetComponent<RectTransform>();
        r.anchorMin = new Vector2(0, 0);
        r.anchorMax = new Vector2(0, 0);
        r.pivot = new Vector2(0, 1);
        r.anchoredPosition = new Vector2(pos.x, pos.y);
    }

    void Finish()
    {
        agregator.anchoredPosition = new Vector2(-player.currentTile.y * 10 + 5, player.currentTile.x * 10 + 5);

        Destroy(this);
    }

}
