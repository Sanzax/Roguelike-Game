using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] Camera minimapCamera;

    public RoomBehaviour CurrentRoom { get; set; }

    public void CenterMinimap(Vector3 position)
    {
        minimapCamera.transform.position = new Vector3(position.x, position.y, minimapCamera.transform.position.z);
    }
}
