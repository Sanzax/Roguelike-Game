using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Vector2 offset;
    bool verticalBoundary, horizontalBoundary;

    Transform cameraParent;

    private void Awake()
    {
        cameraParent = Camera.main.transform.parent;
    }

    void Update()
    {
        if (!verticalBoundary)
            FollowZ();
        if (!horizontalBoundary)
            FollowX();
    }

    void FollowX()
    {
        cameraParent.position = new Vector3(transform.position.x + offset.x, cameraParent.position.y, cameraParent.position.z);
    }

    void FollowZ()
    {
        cameraParent.position = new Vector3(cameraParent.position.x, cameraParent.position.y, transform.position.z + offset.y);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "CameraVerticalBound")
        {
            verticalBoundary = true;
        }
        if (other.tag == "CameraHorizontalBound")
        {
            horizontalBoundary = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "CameraVerticalBound")
        {
            verticalBoundary = false;
        }
        if (other.tag == "CameraHorizontalBound")
        {
            horizontalBoundary = false;
        }
    }
}
