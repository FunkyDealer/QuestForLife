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

        if (chandelierChance >= Random.Range(0, 101))
        {
            currentProps++;
            chandelierObj.SetActive(true);

            GameObject prefab = PropsDataBase.inst.CeilingLamps[0];
            GameObject o = Instantiate(prefab, chandelierObj.transform.position, Quaternion.identity, chandelierObj.transform);
            o.transform.localScale = prefab.transform.localScale;
        }

        int availableSpots = totalSpotsAvailable; //floor spots still avaialble, each ceiling prop that is not the chandelier takes one away

        if (propChance >= Random.Range(1, 101)) //chance that there will be props in this space
        {
            if (floorPropsChance >= Random.Range(1, 101)) //chance that it is a floor prop
            {
                int FloorPropNr = Random.Range(1, maxFloorProps + 1);
                if (FloorPropNr > availableSpots) FloorPropNr = availableSpots;

                int toActivate = Random.Range(0, totalSpotsAvailable);

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
                if (ceilingChange >= Random.Range(1, 101)) //chance that it is a ceiling prop
                {
                    int ceilingPropsNr = Random.Range(1, maxCeilingProps + 1);
                    if (ceilingPropsNr > totalSpotsAvailable) ceilingPropsNr = totalSpotsAvailable;

                    while (true)
                    {
                        int toActivate = Random.Range(0, totalSpotsAvailable);
                        if (!ceilingPropsSpotList[toActivate].activeSelf && !floorPropsSpotList[toActivate].activeSelf)
                        {
                            ceilingPropsSpotList[toActivate].SetActive(true);
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
