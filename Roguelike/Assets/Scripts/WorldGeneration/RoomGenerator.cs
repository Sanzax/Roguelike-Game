using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;

    [SerializeField] int gridSizeX, gridSizeY;


    // Start is called before the first frame update
    void Start()
    {
        TileBase[] tiles = tilemap.GetTilesBlock(tilemap.cellBounds);

        for(int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Sprite sprite = tilemap.GetSprite(new Vector3Int(x, y, 0));
                if (sprite  == null) continue;
                Debug.Log(sprite.name);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
