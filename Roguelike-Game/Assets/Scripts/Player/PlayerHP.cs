using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    PlayerStatCalculation stats;

    float currentHp;

    private void Awake()
    {
        currentHp = stats.GetHp();
    }

    public void TakeDamage(float dmg)
    {
        currentHp -= dmg;
        Debug.Log("Player HP: " + currentHp);
    }
}
