using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    KeyCode shootingButton = KeyCode.Mouse0;

    const string HORIZONTAL = "Horizontal";
    const string VERTICAL = "Vertical";

    public bool IsShooting { get; private set; }

    public float HorizontalInput { get; private set; }
    public float VerticalInput { get; private set; }

    public Vector3 MousePosition 
    {
        get 
        { 
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    void Update()
    {
        ReceiveAxisInput();
        ReceiveButtonInputs();
    }

    void ReceiveAxisInput()
    {
        HorizontalInput = Input.GetAxisRaw(HORIZONTAL);
        VerticalInput = Input.GetAxisRaw(VERTICAL);
    }

    void ReceiveButtonInputs()
    {
        IsShooting = Input.GetKey(shootingButton);
    }
}
