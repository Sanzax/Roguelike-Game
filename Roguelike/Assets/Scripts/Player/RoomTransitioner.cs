using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
public class RoomTransitioner : MonoBehaviour
{
    [SerializeField] float transitionTime = .5f;

    const string tagName = "RoomSwitch";

    public RoomBehaviour CurrentRoom { get; set; }
    RoomBehaviour newRoom;

    CameraFollow cameraFollow;
    PlayerInputs playerInputs;

    [SerializeField] AnimationCurve cameraMovementCurve;
    [SerializeField] AnimationCurve fadeCurve;

    [SerializeField] Volume postProcessing;
    MotionBlur motionBlur;

    [SerializeField] RawImage fadeImage;

    [SerializeField] bool useMotionBlur, useFade;

    private void Awake()
    {
        postProcessing.profile.TryGet(out motionBlur);
        cameraFollow = GetComponent<CameraFollow>();
        playerInputs = GetComponent<PlayerInputs>();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case tagName + "Up":
                ChangeRoom(DoorDirection.Up);
                break;
            case tagName + "Down":
                ChangeRoom(DoorDirection.Down);
                break;
            case tagName + "Left":
                ChangeRoom(DoorDirection.Left);
                break;
            case tagName + "Right":
                ChangeRoom(DoorDirection.Right);
                break;
            default:
                break;
        }
    }


    public void ChangeRoom(DoorDirection doorDirection)
    {
        newRoom = CurrentRoom.GetNeighbours()[(int)doorDirection];
        if (newRoom == null)
        {
            Debug.Log("Huonetta ei ole olemassa");
            return;
        }

        CurrentRoom.ActivateEnemies(false);

        PathRequestManager.instance.SetGrid(newRoom.pathGrid);

        PlayerToNewRoom(newRoom, doorDirection);

        cameraFollow.SetBounds(newRoom.xMin.position.x, newRoom.xMax.position.x, newRoom.yMin.position.z, newRoom.yMax.position.z);

        newRoom.SetRoomActive(true);

        StartCameraTramsition(doorDirection);
    }

    void PlayerToNewRoom(RoomBehaviour newRoom, DoorDirection doorDirection)
    {
        Vector3 spawnPoint = Vector3.zero;
        if (doorDirection == DoorDirection.Up)
            spawnPoint = newRoom.DownSpawn.position;
        else if (doorDirection == DoorDirection.Down)
            spawnPoint = newRoom.UpSpawn.position;
        else if (doorDirection == DoorDirection.Right)
            spawnPoint = newRoom.LeftSpawn.position;
        else if (doorDirection == DoorDirection.Left)
            spawnPoint = newRoom.RightSpawn.position;

        if(doorDirection == DoorDirection.Up || doorDirection == DoorDirection.Down)
            transform.position = new Vector3(transform.position.x, transform.position.y, spawnPoint.z);
        else
            transform.position = new Vector3(spawnPoint.x, transform.position.y, transform.position.z);
    }

    void StartCameraTramsition(DoorDirection doorDirection)
    {
        Vector3 cameraTarget;

        if (doorDirection == DoorDirection.Up)
        {
            cameraTarget = new Vector3(cameraFollow.CameraParent.position.x, 0, newRoom.yMin.position.z);
        }
        else if (doorDirection == DoorDirection.Down)
        {
            cameraTarget = new Vector3(cameraFollow.CameraParent.position.x, 0, newRoom.yMax.position.z);
        }
        else if (doorDirection == DoorDirection.Right)
        {
            cameraTarget = new Vector3(newRoom.xMin.position.x, 0, cameraFollow.CameraParent.position.z);
        }
        else
        {
            cameraTarget = new Vector3(newRoom.xMax.position.x, 0, cameraFollow.CameraParent.position.z);
        }

        StartCoroutine(CameraTransition(cameraTarget));
    }

    IEnumerator CameraTransition(Vector3 targetPosition)
    {
        if(useMotionBlur)
            motionBlur.active = true;
        playerInputs.InputsEnabled = false;

        cameraFollow.Follow = false;

        float elapsed = 0;

        Vector3 startPosition = new Vector3(cameraFollow.CameraParent.position.x, 0, cameraFollow.CameraParent.position.z);

        targetPosition = new Vector3(targetPosition.x, 0, targetPosition.z);

        while (elapsed < transitionTime)
        {
            elapsed += Time.deltaTime;
            Vector3 position = Vector3.Lerp(startPosition, targetPosition, cameraMovementCurve.Evaluate(elapsed / transitionTime));
            cameraFollow.CameraParent.position = new Vector3(position.x, cameraFollow.CameraParent.position.y, position.z);

            if(useFade)
            {
                Color tempColor = fadeImage.color;
                float alpha = Mathf.Lerp(0f, 1f, fadeCurve.Evaluate(elapsed / transitionTime));
                tempColor.a = alpha;
                fadeImage.color = tempColor;
            }
            yield return null;
        }

        cameraFollow.CameraParent.position = new Vector3(targetPosition.x, cameraFollow.CameraParent.position.y, targetPosition.z);

        cameraFollow.Follow = true;
        CurrentRoom.SetRoomActive(false);
        CurrentRoom = newRoom;

        newRoom.ActivateEnemies(true);

        playerInputs.InputsEnabled = true;

        if(useMotionBlur)
            motionBlur.active = false;
        yield return null;
    }

}
