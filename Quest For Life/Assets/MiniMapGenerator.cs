using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapGenerator : MonoBehaviour
{
    Tile[,] map;

    MiniMapManager manager;

    [SerializeField]
    GameObject TilePrefab;

    RectTransform agregator;

    Player player;

    float TileSize;

    [SerializeField]
    Color RoomColor;
    [SerializeField]
    Color WallColor;
    [SerializeField]
    Color chestColor;
    [SerializeField]
    Color ShopColor;

    [SerializeField]
    GameObject StairPrefab;

    public void Init(Tile[,] map, MiniMapManager manager, Player player, float TileSize)
    {
        this.TileSize = TileSize;
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
                switch (map[x,y].type)
                {
                    case Tile.Type.room:                        
                        Image r = InstantiateTile(new Vector2(x, y));
                        r.color = RoomColor;

                        switch (map[x,y].feature)
                        {
                            case Tile.Feature.Exit:
                                Instantiate(StairPrefab, r.gameObject.transform);
                                break;
                            case Tile.Feature.LockedExit:
                                Instantiate(StairPrefab, r.gameObject.transform);
                                break;
                            case Tile.Feature.Fountain:
                                break;
                            case Tile.Feature.Shop:
                                break;
                        }

                        break;
                    case Tile.Type.hall:
                        Image h = InstantiateTile(new Vector2(x, y));
                        h.color = RoomColor;
                        break;
                    case Tile.Type.wall:
                        Image i = InstantiateTile(new Vector2(x, y));

                        switch (map[x,y].feature)
                        {
                            case Tile.Feature.ShopEntrance:
                                i.color = ShopColor;
                                break;
                            case Tile.Feature.Chest:
                                i.color = chestColor;
                                break;
                            case Tile.Feature.ShopExit:
                                i.color = ShopColor;
                                break;
                            case Tile.Feature.None:                                
                                i.color = WallColor;
                                break;
                            default:
                                break;
                        }

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

    Image InstantiateTile(Vector2 pos)
    {
        GameObject o = Instantiate(TilePrefab, agregator.gameObject.transform);
      
        RectTransform r = o.GetComponent<RectTransform>();
        r.sizeDelta = new Vector2(TileSize, TileSize);
        r.anchorMin = new Vector2(0, 0);
        r.anchorMax = new Vector2(0, 0);
        r.pivot = new Vector2(0, 1);
        r.anchoredPosition = new Vector2(pos.x * r.sizeDelta.x, pos.y * r.sizeDelta.y);

        if (!map[(int)pos.x, (int)pos.y].explored) o.SetActive(false);

        manager.tiles.Add(map[(int)pos.x, (int)pos.y], o);

        return o.GetComponent<Image>();
    }

    void Finish()
    {
        agregator.anchoredPosition = new Vector2(-player.currentTile.y * TileSize + TileSize/2, player.currentTile.x * TileSize + TileSize / 2);

        Destroy(this.gameObject);
    }

}
