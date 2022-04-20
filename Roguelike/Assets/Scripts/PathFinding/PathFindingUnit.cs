using System.Collections;
using UnityEngine;

public class PathFindingUnit : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    Vector3[] path;
    int targetIndex;

    public float Speed { get; set; }
    public bool IsMoving { get; private set; }

    public void MoveTo(Vector3 targetPosition)
    {
        PathRequestManager.instance.RemoveAllRequestsFrom(this);
        PathRequestManager.RequestPath(this, transform.position, targetPosition, OnPathFound);
    }

    public void Stop()
    {
        StopCoroutine("FollowPath");
        IsMoving = false;
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
        IsMoving = true;
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
                    IsMoving = false;
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, Speed * Time.fixedDeltaTime);
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
