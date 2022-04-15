using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerHP>().TakeDamage(damage);
        }
        gameObject.SetActive(false);
    }

}
