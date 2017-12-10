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

    public static Map CreateRandomMap2(int width, int height, int numberOfCorridors)
    {
        Map retMap = new Map(width, height);
        
        Corridor latestCorr = retMap.AddCorridor(0,0);

        for (int i = 0; i <= numberOfCorridors; i++)
        {
            // Does the latest corridor have any free sides? We'll use those then
            var availableSides = new HashSet<MapVector2>();

            Debug.Log("Number of corridors: " + i);

            
            var freeNeighbours = latestCorr.GetFreeNeighbours();
            foreach (var c in freeNeighbours)
            {
                int sides = 0;
                foreach (var s in Map.SidesOfCoord(c))
                    if (!retMap.Taken(s.X, s.Y)) sides++;
                if(sides > 1)
                    availableSides.Add(c);
            }
            

            if (availableSides.Count == 0)
            {
                var corrs = retMap.GetAllCorridors();

                foreach (var corr in corrs)
                {
                    availableSides.UnionWith(corr.GetFreeNeighbours());
                }
            }

            if (availableSides.Count == 0)
                break;


            var weightedSides = new List<WeightHolder<MapVector2>>();
            float totalWeight = 0f;

            foreach(var coord in availableSides)
            {
                var wc = new WeightHolder<MapVector2>();
                wc.Item = coord;

                wc.Weight = 0f;
                foreach(var c in Map.SidesOfCoord(coord))
                {
                    if (!retMap.Taken(c.X, c.Y)) wc.Weight += 10.0f;
                }

                if (wc.Weight < 15.001f)
                    continue;

                weightedSides.Add(wc);
                totalWeight += wc.Weight;
            }

            int n = weightedSides.Count;
            while(n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                var value = weightedSides[k];
                weightedSides[k] = weightedSides[n];
                weightedSides[n] = value;
            }

            float randWeightIndex = Random.Range(0f, totalWeight);
            float currentWeight = 0f;
            var newCorr = new MapVector2 { X = -1, Y = -1 };
            Debug.Log("Weighted sides count: " + weightedSides.Count);
            Debug.Log("Total weight: " + totalWeight);
            Debug.Log("randWeightIndex: " + randWeightIndex);

            foreach(var ws in weightedSides)
            {
                Debug.Log("weight: " + ws.Weight);
                currentWeight += ws.Weight;
                if (currentWeight >= randWeightIndex)
                    newCorr = ws.Item;
            }



            Debug.Log("Adding corridor " + newCorr.X + ", " + newCorr.Y);
            latestCorr = retMap.AddCorridor(newCorr);
        }
        Debug.Log("Total corridors created: " + retMap.GetAllCorridors().Count);
        maps.Add(retMap);
        return retMap;
    }
}
