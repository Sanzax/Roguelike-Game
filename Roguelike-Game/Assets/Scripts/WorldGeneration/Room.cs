using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    Minimap miniMap;
    [SerializeField] SpriteRenderer minimapIcon;

    private void Awake()
    {
        miniMap = GameObject.FindGameObjectWithTag("Minimap").GetComponent<Minimap>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            miniMap.CenterMinimap(transform.position);
            CurrentRoomHighlight();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
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


}
