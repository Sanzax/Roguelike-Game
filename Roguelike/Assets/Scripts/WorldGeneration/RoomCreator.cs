using System.Collections.Generic;
using UnityEngine;


public struct RoomLayout
{
    public RoomType RoomType { get; private set; }
    public int  GridId { get; private set; }
    public bool Up { get; private set; }
    public bool Down { get; private set; }
    public bool Left { get; private set; }
    public bool Right { get; private set; }

    public RoomLayout(RoomType type, int gridId, bool up, bool down, bool left, bool right)
    {
        RoomType = type;
        GridId = gridId;
        Up = up;
        Down = down;
        Left = left;
        Right = right;
    }
}

[RequireComponent(typeof(WorldGenerator))]
public class RoomCreator : MonoBehaviour
{
    [SerializeField] RoomLists roomLists;
    [SerializeField] int roomTileWidth = 3;
    [SerializeField] int  roomTileHeight = 3;
    [SerializeField] int roomTileCountX = 40;
    [SerializeField] int roomTileCountY = 30;

    WorldGenerator worldGenerator;

    RoomLayout[,] RoomLayouts;

    GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        worldGenerator = GetComponent<WorldGenerator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void CreateRooms()
    {
        DeleteRooms();
        RoomLayouts = ConvertToRoomLayouts();

        for (int x = 0; x < worldGenerator.GridSizeX; x++)
        {
            for (int y = 0; y < worldGenerator.GridSizeY; y++)
            {
                RoomLayout roomLayout = RoomLayouts[x, y];

                Vector3 offsetPosition = -new Vector3(  (worldGenerator.GridSizeX * roomTileWidth * roomTileCountX - roomTileWidth * roomTileCountX) /2, 1,
                                                        (worldGenerator.GridSizeY * roomTileHeight * roomTileCountY - roomTileHeight * roomTileCountY) /2);
                RoomType roomType = roomLayout.RoomType;
                if (roomType != RoomType.empty)
                {
                    GameObject roomPrefab;

                    /*
                    if (roomLayout.Up && roomLayout.Down && roomLayout.Left && roomLayout.Right) roomPrefab = roomLists.roomsUDLR[0];
                    else if (!roomLayout.Up && roomLayout.Down && !roomLayout.Left && !roomLayout.Right) roomPrefab = roomLists.roomsD[0];
                    else if (!roomLayout.Up && !roomLayout.Down && roomLayout.Left && !roomLayout.Right) roomPrefab = roomLists.roomsL[0];
                    else if (!roomLayout.Up && !roomLayout.Down && !roomLayout.Left && roomLayout.Right) roomPrefab = roomLists.roomsR[0];
                    else if (roomLayout.Up && !roomLayout.Down && !roomLayout.Left && !roomLayout.Right) roomPrefab = roomLists.roomsU[0];
                    else if (!roomLayout.Up && roomLayout.Down && roomLayout.Left && !roomLayout.Right) roomPrefab = roomLists.roomsDL[0];
                    else if (!roomLayout.Up && roomLayout.Down && !roomLayout.Left && roomLayout.Right) roomPrefab = roomLists.roomsDR[0];
                    else if (!roomLayout.Up && !roomLayout.Down && roomLayout.Left && roomLayout.Right) roomPrefab = roomLists.roomsLR[0];
                    else if (roomLayout.Up && roomLayout.Down && !roomLayout.Left && !roomLayout.Right) roomPrefab = roomLists.roomsUD[0];
                    else if (roomLayout.Up && !roomLayout.Down && roomLayout.Left && !roomLayout.Right) roomPrefab = roomLists.roomsUL[0];
                    else if (roomLayout.Up && !roomLayout.Down && !roomLayout.Left && roomLayout.Right) roomPrefab = roomLists.roomsUR[0];
                    else if (!roomLayout.Up && roomLayout.Down && roomLayout.Left && roomLayout.Right) roomPrefab = roomLists.roomsDRL[0];
                    else if (roomLayout.Up && roomLayout.Down && roomLayout.Left && !roomLayout.Right) roomPrefab = roomLists.roomsUDL[0];
                    else if (roomLayout.Up && roomLayout.Down && !roomLayout.Left && roomLayout.Right) roomPrefab = roomLists.roomsUDR[0];
                    else roomPrefab = roomLists.roomsULR[0];
                    */
                    roomPrefab = roomLists.roomsULR[0];

                    Vector3 roomPosition = new Vector3(x * roomTileWidth * roomTileCountX, 0, y * roomTileHeight * roomTileCountY);
                    GameObject roomObj = Instantiate(roomPrefab, roomPosition + offsetPosition, Quaternion.identity, transform);
                    RoomBehaviour roomBehaviour = roomObj.GetComponent<RoomBehaviour>();
                    roomBehaviour.GridId = roomLayout.GridId;

                    //roomBehaviour.pathGrid.CreateGrid();

                    if(roomType == RoomType.start)
                    {
                        player.GetComponent<RoomTransitioner>().CurrentRoom = roomBehaviour;
                        player.GetComponent<CameraFollow>().SetBounds(roomBehaviour.xMin.position.x, 
                                                                        roomBehaviour.xMax.position.x,
                                                                        roomBehaviour.yMin.position.z, 
                                                                        roomBehaviour.yMax.position.z);

                        PathRequestManager.instance.SetGrid(roomBehaviour.pathGrid);
                    }
                    else
                    {
                        roomBehaviour.SetRoomActive(false);
                    }
                }
            }
        }
    }

    void DeleteRooms()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }


    RoomLayout[,] ConvertToRoomLayouts()
    {
        RoomLayout[,] roomLayouts = new RoomLayout[worldGenerator.GridSizeX, worldGenerator.GridSizeY];

        for(int x = 0; x < worldGenerator.GridSizeX; x++)
        {
            for (int y = 0; y < worldGenerator.GridSizeX; y++)
            {
                roomLayouts[x, y] = GetRoomLayoutFrom(x, y);
            }
        }

        return roomLayouts;
    }

    RoomLayout GetRoomLayoutFrom(int x, int y)
    {
        RoomLayout roomLayout;

        RoomType roomType = (RoomType)worldGenerator.Rooms[x, y];
        int gridId = x + y * worldGenerator.GridSizeX;
        if (roomType == RoomType.empty)
        {
            return new RoomLayout(roomType, gridId, false, false, false, false);
        }

        bool up = false, down = false, right = false, left = false;

        List<XY> neighbours = worldGenerator.GetNeighbours(new XY(x, y));

        foreach(XY neighbour in neighbours)
        {
            if (worldGenerator.Rooms[neighbour.x, neighbour.y] == (int)RoomType.empty)
                continue;

            if (neighbour.x - x == 1)
                right = true;
            else if (neighbour.x - x == -1)
                left = true;
            else if (neighbour.y - y == -1)
                down = true;
            else if (neighbour.y - y == 1)
                up = true;
        }

        roomLayout = new RoomLayout(roomType, gridId, up, down, left, right);

        return roomLayout;
    }

}
