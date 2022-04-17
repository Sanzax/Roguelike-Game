using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.transform.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyHP>().TakeDamage(damage);
        }
        gameObject.SetActive(false);
    }

}
