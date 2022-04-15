using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] PlayerInputs playerInputs;
    [SerializeField] PlayerStatCalculation stats;
    [SerializeField] string bulletPool;
    [SerializeField] Transform gun;
    [SerializeField] Transform bulletSpawnPoint;

    float shootTimer;

    void Update()
    {
        RotateGun();

        shootTimer += Time.deltaTime;
        if (CanShoot())
        {
            ShootingPatterns.ShootTowardsTarget(playerInputs.MousePosition, transform.position, Shoot, 1, 0f);
        }
    }

    bool CanShoot()
    {
        return playerInputs.IsShooting && shootTimer > stats.GetShootDelay();
    }

    void RotateGun()
    {
        Vector3 direction = (playerInputs.MousePosition - gun.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gun.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Shoot(float angle)
    {
        shootTimer = 0;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        GameObject projectileObj = ObjectPooler.Instance.SpawnFromPool(bulletPool, bulletSpawnPoint.position, rotation);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        projectile.Init(stats.GetShotSpeed(), stats.GetDamage());
    }
}
