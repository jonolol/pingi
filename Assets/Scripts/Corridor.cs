using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corridor : MonoBehaviour
{
    public MapVector2 MapCoordinate;
    protected float corridorLength = 10f;
    protected Dictionary<Direction, Connection> connections = new Dictionary<Direction, Connection>();
    protected bool initialized = false;
    public GameObject SideNorth;
    public GameObject SideEast;
    public GameObject SideSouth;
    public GameObject SideWest;
    public GameObject SideUp;
    public GameObject SideDown;

    public class Connection
    {
        public Direction Direction;
        public bool Connected;
        public Corridor Connector;
        public Direction ConnectorDirection;
        public GameObject WallObject;

        public Connection()
        {
            Direction = 0;
            Connected = false;
            Connector = null;
            ConnectorDirection = 0;
            WallObject = null;
        }

    }

    public enum Direction
    {
        North,
        East,
        South,
        West,
        Up,
        Down
    }

    public float Length
    {
        get { return corridorLength; }
    }

    public Vector3 GetOffset(Direction side)
    {
        switch (side)
        {
            case Direction.North: return new Vector3(-1f,  0f,  0f);
            case Direction.East:  return new Vector3( 0f,  0f,  1f);
            case Direction.South: return new Vector3( 1f,  0f,  0f);
            case Direction.West:  return new Vector3( 0f,  0f, -1f);
            case Direction.Up:    return new Vector3( 0f,  1f,  0f);
            case Direction.Down:  return new Vector3( 0f, -1f,  0f);
            default:              return Vector3.zero;
        }
    }

    public bool AttachCorridor(Direction dirThis, Direction dirThat, Corridor corr)
    {
        if (!connections.ContainsKey(dirThis) ||
            !corr.connections.ContainsKey(dirThat))
        {
            Debug.LogError("One of the corridors do not contain that direction");
            return false;
        }

        Connection cThis = connections[dirThis];
        Connection cThat = corr.connections[dirThat];

        if (cThis.Connected || cThat.Connected)
        {
            Debug.LogError("One of the corridors is already connected");
            return false;
        }

        GameObject objThis = this.gameObject;
        GameObject objThat = corr.gameObject;

        cThis.Connected = true;
        cThis.Connector = corr;
        cThis.Direction = dirThis;
        cThis.ConnectorDirection = dirThat;
        this.RemoveSide(dirThis);

        cThat.Connected = true;
        cThat.Connector = this;
        cThat.Direction = dirThat;
        cThat.ConnectorDirection = dirThis;
        corr.RemoveSide(dirThat);

        objThat.transform.position = objThis.transform.position + (GetOffset(dirThis) * corridorLength);

        Debug.LogError("Attaching a corridor from " + dirThis.ToString() + " | to " + dirThat.ToString());
        Debug.LogError("Setting to position: " + objThat.transform.position);

        // TODO: Fix rotations and stuff in case of things like north to east

        return true;
    }

    public void RemoveSide(Direction dir)
    {
        // TODO: Make wall invisible and intangible instead of destroying it

        Destroy(connections[dir].WallObject);
    }

    public bool RemoveCorridor(Direction dir)
    {
        return false;
        /*

        if (!connections.ContainsKey(dir))
            return false;

        Connection cThis = connections[dirThis];
        Connection cThat = corr.connections[dirThat];

        if (cThis.connected || cThat.connected)
            return false;

        return true;
        */
    }

    public virtual bool Initialize()
    {
        if (initialized) return false;

        connections = new Dictionary<Direction, Connection>();
        foreach (Direction currentDirection in System.Enum.GetValues(typeof(Direction)))
        {
            connections[currentDirection] = new Connection() { Direction = currentDirection };
        }
        connections[Direction.North].WallObject = SideNorth;
        connections[Direction.East ].WallObject = SideEast;
        connections[Direction.South].WallObject = SideSouth;
        connections[Direction.West ].WallObject = SideWest;
        connections[Direction.Up   ].WallObject = SideUp;
        connections[Direction.Down ].WallObject = SideDown;

        initialized = true;
        return true;
    }

    private void Awake()
    {
        Initialize();
    }

    // Use this for initialization
    void Start()
    {
        Initialize();
    }
}
