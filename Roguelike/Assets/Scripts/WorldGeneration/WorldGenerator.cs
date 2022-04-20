using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct XY
{
    public int x;
    public int y;

    public XY(int _x, int _y) 
    {
        x = _x;
        y = _y;
    }
}

public enum RoomType
{
    empty,
    start,
    normal,
    boss,
    item,
    shop
}

public class WorldGenerator : MonoBehaviour
{
    RoomCreator roomCreator;

	public int[,] Rooms { get; private set; }

    [SerializeField] int gridSizeX = 15, gridSizeY = 15;
    public int GridSizeX { get { return gridSizeX; } }
    public int GridSizeY { get { return gridSizeY; } }

    [SerializeField] int maxRoomCount = 15;
    [SerializeField] int maxBranchSize = 2;
    [SerializeField] int minBranchSize = 1;
    [SerializeField] int maxMainBranchSize = 8;
    [SerializeField] int minMainBranchSize = 6;
    [SerializeField] float bracnhingProbability = 1.5f;
    int currentRoomCount;

    bool isBossRoomGenerated;
    bool isShopRoomGenerated;
    bool isItemRoomGenerated;

    List<XY> deadEnds;
    Stack<XY> stack;

    [SerializeField] bool displayGizmos;
    [SerializeField] bool createRooms;


    private void Start()
    {
        roomCreator = GetComponent<RoomCreator>();

        Rooms = new int[gridSizeX, gridSizeY];
        /*for(int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Rooms[x, y] = (int)RoomType.normal;
            }
        }*/


        GenerateMap();
        if(createRooms)
            roomCreator.CreateRooms();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GenerateMap();
            if (createRooms)
                roomCreator.CreateRooms();
        }
    }

    void GenerateMap()
    {
        Rooms = new int[gridSizeX, gridSizeY];
        stack = new Stack<XY>();
        currentRoomCount = 0;

        deadEnds = new List<XY>();

        isBossRoomGenerated = false;
        isItemRoomGenerated = false;
        isShopRoomGenerated = false;

        // Make first room in center of the map
        int currentX = (gridSizeX - 1) / 2;
        int currentY = (gridSizeY - 1) / 2;

        AddRoom(new XY(currentX, currentY), (int)RoomType.start);
        Branch(maxMainBranchSize-1, minMainBranchSize);

        int i = 0;
        while(stack.Count > 0 && i < 1000)
        {
            i++;
            float r = Random.Range(0f, 1f);
            if(r > bracnhingProbability / stack.Count)
            {
                stack.Pop();
                continue;
            }

            Branch(maxBranchSize, minBranchSize);
            if (currentRoomCount == maxRoomCount)
            {
                AddSpecialRooms();
                return;
            }
        }

        AddSpecialRooms();
    }

    void Branch(int maxSize, int minSize)
    {
        int size = Random.Range(minSize, maxSize+1);
        for (int i = 0; i < size; i++)
        {
            List<XY> neighbours = GetNeighbours(stack.Peek());
            if (!ContinueBranchToRandomNeighbour(neighbours))
            {
                stack.Pop();
                return;
            }

        }

        deadEnds.Add(new XY(stack.Peek().x, stack.Peek().y));
        stack.Pop();
    }

    void AddSpecialRooms()
    {
        XY start = new XY((gridSizeX - 1) / 2, (gridSizeY - 1) / 2);
        int pathLength = PathLengthFromTo(start, deadEnds[0]);

        Rooms[deadEnds[0].x, deadEnds[0].y] = (int)RoomType.boss;
        deadEnds.RemoveAt(0);

        deadEnds = deadEnds.OrderBy(o => PathLengthFromTo(start, o)).ToList();

        for (int i = 0; i < deadEnds.Count; i ++)
        {
            if(!isItemRoomGenerated)
            {
                Rooms[deadEnds[i].x, deadEnds[i].y] = (int)RoomType.item;
                isItemRoomGenerated = true;
            }
            else if (!isShopRoomGenerated)
            {
                Rooms[deadEnds[i].x, deadEnds[i].y] = (int)RoomType.shop;
                isShopRoomGenerated = true;
            }
        }
    }

    bool ContinueBranchToRandomNeighbour(List<XY> neighbours)
    {
        while (neighbours.Count > 0)
        {
            int index = Random.Range(0, neighbours.Count);

            if (!IsValidPosition(neighbours[index]))
            {
                neighbours.RemoveAt(index);
                continue;
            }
            else
            {
                AddRoom(neighbours[index], (int)RoomType.normal);
                if (currentRoomCount == maxRoomCount)
                {
                    return false;
                }
                break;
            }
        }

        if (neighbours.Count == 0)
        {
            return false;
        }

        return true;
    }

    int nn = 0;
    int PathLengthFromTo(XY start, XY end)
    {
        nn = 0;
        if (Rooms[start.x, start.y] == (int)RoomType.empty || Rooms[end.x, end.y] == (int)RoomType.empty)
            return 0;

        int pathLength = 0;

        Stack<XY> xyStack = new Stack<XY>();
        xyStack.Push(start);

        LoopNeighbours(start, end, new XY(-1, -1), ref pathLength, xyStack);
        
        return pathLength;
    }

    bool LoopNeighbours(XY current, XY end, XY last, ref int pathLength, Stack<XY> xyStack)
    {
        nn++;
        if(current.x  == end.x && current.y == end.y || nn > 100)
        {
            return true;
        }

        List<XY> neighbours = GetNeighbours(current);
        neighbours.Remove(last);

        for(int i = 0; i < neighbours.Count; i++)
        {
            XY neighbour = neighbours[i];
            if (HasRoom(neighbour))
            {
                xyStack.Push(neighbour);
                pathLength++;
                if(LoopNeighbours(neighbour, end, current, ref pathLength, xyStack))
                {
                    return true;
                }
            }
        }
        xyStack.Pop();
        pathLength--;
        return false;
    }

    void AddRoom(XY xy, int type)
    {
        Rooms[xy.x, xy.y] = type;
        stack.Push(new XY(xy.x, xy.y));
        currentRoomCount++;
    }

    public List<XY> GetNeighbours(XY xy)
    {
        List<XY> neighbours = new List<XY>();

        for(int i = 0; i < 4; i++)
        {
            XY current;
            if (i == 0) current = new XY(xy.x + 1, xy.y);
            else if (i == 1) current = new XY(xy.x - 1, xy.y);
            else if (i == 2) current = new XY(xy.x, xy.y + 1);
            else current = new XY(xy.x, xy.y - 1);

            if (current.x < gridSizeX && current.x > -1 && current.y < gridSizeY && current.y > -1)
                neighbours.Add(current);
        }

        return neighbours;
    }

    bool IsValidPosition(XY xy)
    {
        if(HasRoom(xy) || GetOccupiedNeighbourCount(xy) > 1)
        {
            return false;
        }

        return true;
    }

    bool HasRoom(XY xy)
    {
        if (xy.x < 0 || xy.x > gridSizeX - 1 || xy.y < 0 || xy.y > gridSizeY - 1 || Rooms[xy.x, xy.y] == 0)
        {
            return false;
        }

        return true;
    }

    int GetOccupiedNeighbourCount(XY xy)
    {
        int occupied = 0;

        List<XY> neighbours = GetNeighbours(xy);

        foreach(XY neighbour in neighbours)
        {
            if(Rooms[neighbour.x, neighbour.y] != (int)RoomType.empty)
            {
                occupied++;
            }
        }

        return occupied;
    }

    private void OnDrawGizmos()
    {
        if(Rooms != null && displayGizmos)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    if (Rooms[x, y] == (int)RoomType.empty)
                        Gizmos.color = Color.white;
                    else if (Rooms[x, y] == (int)RoomType.normal)
                        Gizmos.color = Color.black;
                    else if (Rooms[x, y] == (int)RoomType.start)
                        Gizmos.color = Color.cyan;
                    else if (Rooms[x, y] == (int)RoomType.boss)
                        Gizmos.color = Color.red;
                    else if (Rooms[x, y] == (int)RoomType.item)
                        Gizmos.color = Color.yellow;
                    else if (Rooms[x, y] == (int)RoomType.shop)
                        Gizmos.color = Color.green;
                    Vector3 pos = transform.position + new Vector3(x, y, 0);
                    Gizmos.DrawCube(pos, Vector3.one * 0.9f);
                }
            }
        }
    }

}
