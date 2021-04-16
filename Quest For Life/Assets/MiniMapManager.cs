using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapManager : MonoBehaviour
{
    Tile[,] map;

    [SerializeField]
    NavigationInterfaceManager interfaceManager;

    [SerializeField]
    GameObject miniMapGeneratorPrefab;


    public RectTransform agregator;


    void Awake()
    {
        Player.onPlayerMove += MoveWithPlayer;
    }

    void OnDestroy()
    {
        Player.onPlayerMove -= MoveWithPlayer;
    }

    // Start is called before the first frame update
    void Start()
    {
        map = interfaceManager.player.currentMap;

        agregator.sizeDelta = new Vector2(10 * map.GetLength(0), 10 * map.GetLength(1));

        GenerateMiniMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateMiniMap()
    {
        GameObject o = Instantiate(miniMapGeneratorPrefab, this.gameObject.transform);
        MiniMapGenerator m = o.GetComponent<MiniMapGenerator>();
        m.Init(map, this, interfaceManager.player);


    }


    void MoveWithPlayer(Vector2 newPos)
    {
        agregator.anchoredPosition = new Vector2(-newPos.y * 10 + 5, newPos.x * 10 + 5);


    }

}
