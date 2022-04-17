using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    EnemyStatCalculation stats;

    float currentHp;

    private void Awake()
    {
        currentHp = stats.GetHp();
    }

    public void TakeDamage(float dmg)
    {
        currentHp -= dmg;
        Debug.Log("Enemy HP: " + currentHp);
    }
}
