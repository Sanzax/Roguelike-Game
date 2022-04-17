using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingUnit : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    Vector3[] path;
    int targetIndex;

    public float Speed { get; set; }

    public void MoveTo(Vector3 targetPosition)
    {
        PathRequestManager.RequestPath(transform.position, targetPosition, OnPathFound);
    }

    private void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if(pathSuccessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        if (path.Length == 0)
            yield break;
        Vector3 currentWaypoint = path[0];

        while(true)
        {
            if(rb.position == currentWaypoint)
            {
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, Speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if(path != null)
        {
            for(int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one * .25f);

                if(i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i-1], path[i]);
                }
            }
        }
    }
}
