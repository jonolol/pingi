using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MapVector2
{
    public int X;
    public int Y;

    public MapVector2(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
}

public class Map : MonoBehaviour
{
    public Corridor[,] CorridorArray;
    public int Width;
    public int Height;
    private const string prefabName = "CorridorCube";
    private Vector2 worldSpaceOrigin = new Vector2(0f, 0f);

    public Map(int width, int height)
    {
        this.Width  = width;
        this.Height = height;

        CorridorArray = new Corridor[Width, Height];
    }


    public Corridor AddCorridor(MapVector2 position)
    {
        return AddCorridor(position.X, position.Y);
    }

    public Corridor AddCorridor(int x, int y)
    {
        if (CorridorArray[x, y] != null)
        {
            return null;
        }

        GameObject gameObject = (GameObject)Instantiate(Resources.Load(prefabName));
        Corridor corr = gameObject.GetComponent<Corridor>();
        CorridorArray[x, y] = corr;
    
        corr.MapCoordinate.X = x;
        corr.MapCoordinate.Y = y;

        gameObject.transform.position = new Vector3(x * corr.Length, 0, y * corr.Length);

        if (x > 0 && CorridorArray[x - 1, y] != null)
        {
            Corridor westCorridor = CorridorArray[x - 1, y];
            westCorridor.AttachCorridor(Corridor.Direction.East, Corridor.Direction.West, corr);
        }
        if (x < Width && CorridorArray[x + 1, y] != null)
        {
            Corridor eastCorridor = CorridorArray[x + 1, y];
            eastCorridor.AttachCorridor(Corridor.Direction.West, Corridor.Direction.East, corr);
        }
        if (y > 0 && CorridorArray[x, y - 1] != null)
        {
            Corridor southCorridor = CorridorArray[x, y - 1];
            southCorridor.AttachCorridor(Corridor.Direction.North, Corridor.Direction.South, corr);
        }
        if (y < Height && CorridorArray[x, y + 1] != null)
        {
            Corridor northCorridor = CorridorArray[x, y + 1];
            northCorridor.AttachCorridor(Corridor.Direction.South, Corridor.Direction.North, corr);
        }

        return corr;
    }

    public bool Taken(int x, int y)
    {
        if (x < 0 || x > Width || y < 0 || y > Height)
            return true;

        return CorridorArray[x, y] != null;
    }

    public List<Corridor> GetAllCorridors()
    {
        var corridors = new List<Corridor>();

        foreach(var c in CorridorArray)
        {
            if (c != null) corridors.Add(c);
        }

        return corridors;
    }
}
