using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerInputs playerInputs;
    [SerializeField] Rigidbody rb;
    [SerializeField] PlayerStatCalculation stats;

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    void Move()
    {
        Vector3 moveDirection = new Vector3(playerInputs.HorizontalInput, 0, playerInputs.VerticalInput).normalized;
        rb.velocity = moveDirection * stats.GetSpeed();
    }

    void Rotate()
    {
        rb.rotation = Quaternion.Euler(new Vector3(0, playerInputs.Angle, 0));
    }
}
