using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    protected static List<Map> maps = new List<Map>();

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public static Map CreateRandomMap(int width, int height, int numberOfCorridors)
    {
        Map retMap = new Map(width, height);

        retMap.AddCorridor(0, 0);

        for (int i = 0; i <= numberOfCorridors; i++)
        {
            var corrs = retMap.GetAllCorridors();
            var availableSides = new HashSet<MapVector2>();

            foreach (var corr in corrs)
            {
                int x = corr.MapCoordinate.X;
                int y = corr.MapCoordinate.Y;
                if (!retMap.Taken(x - 1, y)) availableSides.Add(new MapVector2(x - 1, y));
                if (!retMap.Taken(x + 1, y)) availableSides.Add(new MapVector2(x + 1, y));
                if (!retMap.Taken(x, y - 1)) availableSides.Add(new MapVector2(x, y - 1));
                if (!retMap.Taken(x, y + 1)) availableSides.Add(new MapVector2(x, y + 1));
            }

            if (availableSides.Count == 0)
                return null;

            int randIndex = Random.Range(0, availableSides.Count);
            MapVector2 randVec = availableSides.ElementAt(randIndex);
            retMap.AddCorridor(randVec);
        }
        maps.Add(retMap);

        return retMap;
    }
}
