using UnityEngine;
using UnityEngine.AI;

public enum DoorDirection
{
    Up,
    Right,
    Down,
    Left
}

public class RoomBehaviour : MonoBehaviour
{
    Minimap miniMap;
    [SerializeField] SpriteRenderer minimapIcon;

    [SerializeField] Transform enemies;
    RoomType roomType;

    //NavMehSurface navMehSurface;

    // 0, up
    // 1, right
    // 2, down
    // 3 left
    RoomBehaviour[] neighbours;
    public int GridId { get; set; }

    WorldGenerator worldGenerator;

    public Transform UpSpawn, DownSpawn, RightSpawn, LeftSpawn;

    public Transform xMin, xMax, yMin, yMax;

    public PathGrid pathGrid;


    private void Awake()
    {
        worldGenerator = transform.parent.GetComponent<WorldGenerator>();
        miniMap = GameObject.FindGameObjectWithTag("Minimap").GetComponent<Minimap>();
    }


    public RoomBehaviour[] GetNeighbours()
    {
        if (neighbours != null)
            return neighbours;
        Debug.Log("Ekaa kertaa tääl");
        neighbours = new RoomBehaviour[4];

        foreach (Transform room in transform.parent)
        {
            if (room == transform) continue;

            RoomBehaviour roomBehaviour = room.GetComponent<RoomBehaviour>();

            if(roomBehaviour.GridId == GridId + worldGenerator.GridSizeX)
            {
                neighbours[0] = roomBehaviour;
            }
            else if(roomBehaviour.GridId == GridId + 1)
            {
                neighbours[1] = roomBehaviour;
            }

            else if (roomBehaviour.GridId == GridId - worldGenerator.GridSizeX)
            {
                neighbours[2] = roomBehaviour;
            }

            else if (roomBehaviour.GridId == GridId - 1)
            {
                neighbours[3] = roomBehaviour;
            }
        }

        return neighbours;
    }

    public void SetRoomActive(bool flag)
    {
        transform.GetChild(0).gameObject.SetActive(flag);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.tag == "Player")
        {
            miniMap.CenterMinimap(transform.position);
            CurrentRoomHighlight();
        }

    }

    private void OnTriggerExit(Collider other)
    {

        if (other.transform.tag == "Player")
        {
            NormalColor();
        }

    }

    void CurrentRoomHighlight()
    {
        minimapIcon.color = Color.grey;
    }

    void NormalColor()
    {
        minimapIcon.color = Color.white;
    }

    public void ActivateEnemies(bool flag)
    {
        foreach(Transform enemyTransform in enemies)
        {
            EnemyStateMachine enemy = enemyTransform.GetComponent<EnemyStateMachine>();
            enemy.IsActive = flag;
        }
    }
}
