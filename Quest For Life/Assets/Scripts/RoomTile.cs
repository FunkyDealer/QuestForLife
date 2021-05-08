using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomTile : PhysicalTile
{
    [SerializeField]
    List<GameObject> floorPropsSpotList;
    [SerializeField]
    List<GameObject> ceilingPropsSpotList;
    [SerializeField]
    GameObject chandelierObj;

    [SerializeField]
    int maxMajorProps = 1;
    [SerializeField]
    int maxFloorProps = 1;
    [SerializeField]
    int maxCeilingProps = 1;

    [SerializeField]
    int propChance = 20;

    [SerializeField]
    int chandelierChance = 1; //chance of chandelier
    [SerializeField]
    int floorPropsChance = 50; //chance that there will be props on the floor
    [SerializeField]
    int ceilingChange = 10; //chance that there will be ceiling props

    public MapManager m;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var f in floorPropsSpotList) f.SetActive(false);
        foreach (var c in ceilingPropsSpotList) c.SetActive(false);
        chandelierObj.SetActive(false);
        
        PropBuilder();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PropBuilder()
    {
        int currentProps = 0;
        int totalSpotsAvailable = ceilingPropsSpotList.Count; //number of spots in the ceiling

        if (chandelierChance >= Global.Range(0, 101, m))
        {
            currentProps++;
            chandelierObj.SetActive(true);

            GameObject prefab = PropsDataBase.inst.CeilingLamps[0];
            GameObject o = Instantiate(prefab, chandelierObj.transform.position, Quaternion.identity, chandelierObj.transform);
            o.transform.localScale = prefab.transform.localScale;
        }

        int availableSpots = totalSpotsAvailable; //floor spots still avaialble, each ceiling prop that is not the chandelier takes one away

        if (propChance >= Global.Range(1, 101, m)) //chance that there will be props in this space
        {
            if (floorPropsChance >= Global.Range(1, 101, m)) //chance that it is a floor prop
            {
                int FloorPropNr = Global.Range(1, maxFloorProps + 1, m);
                if (FloorPropNr > availableSpots) FloorPropNr = availableSpots;

                int toActivate = Global.Range(0, totalSpotsAvailable, m);

                while (true)
                {
                    if (!ceilingPropsSpotList[toActivate].activeSelf && !floorPropsSpotList[toActivate].activeSelf)
                    {
                        floorPropsSpotList[toActivate].SetActive(true);
                        break;
                    }
                }
                availableSpots--;
                currentProps++;
            }
            else
            {
                if (ceilingChange >= Global.Range(1, 101, m)) //chance that it is a ceiling prop
                {
                    int ceilingPropsNr = Global.Range(1, maxCeilingProps + 1, m);
                    if (ceilingPropsNr > totalSpotsAvailable) ceilingPropsNr = totalSpotsAvailable;

                    while (true)
                    {
                        int toActivate = Global.Range(0, totalSpotsAvailable, m);
                        if (!ceilingPropsSpotList[toActivate].activeSelf && !floorPropsSpotList[toActivate].activeSelf)
                        {
                            ceilingPropsSpotList[toActivate].SetActive(true);

                            GameObject prefab = PropsDataBase.inst.CeilingProps[0];
                            GameObject o = Instantiate(prefab, ceilingPropsSpotList[toActivate].transform.position, Quaternion.identity, ceilingPropsSpotList[toActivate].transform);
                            o.transform.localScale = prefab.transform.localScale;

                            break;
                        }
                    }
                    availableSpots--;
                    currentProps++;
                }
            }

        }

    } //end prop builder


    
} //end of class
