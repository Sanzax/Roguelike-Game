using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    public static PathRequestManager instance;
    PathFinding pathFinding;

    bool isProcessingPath;

    private void Awake()
    {
        instance = this;
        pathFinding = GetComponent<PathFinding>();
    }

    public static void RequestPath(PathFindingUnit requester, Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(requester, pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        if(!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathFinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    public void SetGrid(PathGrid newGrid)
    {
        pathFinding.SetGrid(newGrid);
    }

    public void RemoveAllRequestsFrom(PathFindingUnit requester)
    {
        pathRequestQueue = new Queue<PathRequest>(pathRequestQueue.Where(request => request.requester != requester));
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;
        public PathFindingUnit requester;

        public PathRequest(PathFindingUnit _requester, Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
        {
            requester = _requester;
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }
}
