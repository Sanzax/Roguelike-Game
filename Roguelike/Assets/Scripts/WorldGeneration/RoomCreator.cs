using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct RoomLayout
{
    public RoomType RoomType { get; set; }
    public bool Up { get; set; }
    public bool Down { get; set; }
    public bool Left { get; set; }
    public bool Right { get; set; }

    public RoomLayout(RoomType type, bool up, bool down, bool left, bool right)
    {
        RoomType = type;
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
    int roomTileWidth = 18;
    int roomTileHeight = 10;

    WorldGenerator worldGenerator;

    RoomLayout[,] RoomLayouts;

    // Start is called before the first frame update
    void Awake()
    {
        worldGenerator = GetComponent<WorldGenerator>();
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

                Vector3 offsetPosition = -new Vector3(  (worldGenerator.GridSizeX * roomTileWidth - roomTileWidth)/2, 1,
                                                        (worldGenerator.GridSizeY * roomTileHeight - roomTileHeight) /2);

                if (roomLayout.RoomType != RoomType.empty)
                {
                    GameObject roomPrefab;

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

                    Vector3 roomPosition = new Vector3(x * roomTileWidth, 0, y * roomTileHeight);

                    GameObject roomObj = Instantiate(roomPrefab, roomPosition + offsetPosition, Quaternion.identity, transform);
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
        if (roomType == RoomType.empty)
        {
            return new RoomLayout(roomType, false, false, false, false);
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

        roomLayout = new RoomLayout(roomType, up, down, left, right);

        return roomLayout;
    }

}
