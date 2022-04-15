using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerInputs playerInputs;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerStatCalculation stats;

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector2 moveDirection = new Vector2(playerInputs.HorizontalInput, playerInputs.VerticalInput).normalized;
        rb.velocity = moveDirection * stats.GetSpeed();
    }
}
