﻿using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    KeyCode shootingButton = KeyCode.Mouse0;

    const string HORIZONTAL = "Horizontal";
    const string VERTICAL = "Vertical";

    public bool IsShooting { get; private set; }

    public float HorizontalInput { get; private set; }
    public float VerticalInput { get; private set; }

    public bool InputsEnabled { get; set; }

    public float Angle 
    {
        get 
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 5.23f;
            Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
            float angle = Mathf.Atan2(mousePos.y - objectPos.y, mousePos.x - objectPos.x) * Mathf.Rad2Deg;
            return -angle;
        }
    }

    private void Awake()
    {
        InputsEnabled = true;
    }

    void Update()
    {
        if(InputsEnabled)
        {
            ReceiveAxisInput();
            ReceiveButtonInputs();
        }
        else
        {
            IsShooting = false;
            HorizontalInput = 0f;
            VerticalInput = 0f;
        }
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
