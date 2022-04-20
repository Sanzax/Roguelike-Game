using UnityEngine;

public class PlayerProjectile : Projectile
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyHP>().TakeDamage(damage);
        }
        gameObject.SetActive(false);
    }

}
