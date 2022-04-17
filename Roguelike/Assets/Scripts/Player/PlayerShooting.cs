using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] PlayerInputs playerInputs;
    [SerializeField] PlayerStatCalculation stats;
    [SerializeField] string bulletPool;
    [SerializeField] Transform bulletSpawnPoint;

    float shootTimer;

    void Update()
    {
        //RotateGun();

        shootTimer += Time.deltaTime;
        if (CanShoot())
        {
            ShootingPatterns.ShootAtAngle(playerInputs.Angle, Shoot, 1, 0f);
        }
    }

    bool CanShoot()
    {
        return playerInputs.IsShooting && shootTimer > stats.GetShootDelay();
    }

    void Shoot(float angle)
    {
        shootTimer = 0;
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        GameObject projectileObj = ObjectPooler.Instance.SpawnFromPool(bulletPool, bulletSpawnPoint.position, rotation);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        projectile.Init(stats.GetShotSpeed(), stats.GetDamage());
    }
}
