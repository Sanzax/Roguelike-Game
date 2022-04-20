using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    bool verticalBoundary, horizontalBoundary;

    public Transform CameraParent { get; private set; }

    float xMin, xMax, yMin, yMax;

    public bool Follow { get; set; }

    private void Awake()
    {
        Follow = true;
        CameraParent = Camera.main.transform.parent.parent;
        xMin = -20;
        xMax = 20;
        yMin = -20;
        yMax = 20;
    }

    void LateUpdate()
    {
        if(Follow)
        {
            CameraParent.position = new Vector3(
                Mathf.Clamp(transform.position.x, xMin, xMax),
                CameraParent.position.y,
                Mathf.Clamp(transform.position.z, yMin, yMax));
        }

    }


    public void SetBounds(float XMin, float XMax, float YMin, float YMax)
    {
        xMin = XMin;
        xMax = XMax;
        yMin = YMin;
        yMax = YMax;
    }
}
