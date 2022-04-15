using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState_Test : IState
{
    public EnemyStateMachine Machine { get; set; }
    float attackTimer = 0f;

    public AttackState_Test(EnemyStateMachine m)
    {
        Machine = m;
    }

    public void Start()
    {

    }

    public void Update()
    {
        Machine.PathUnit.MoveTo(Machine.Player.transform.position);
        attackTimer += Time.deltaTime;
        if(attackTimer > Machine.Stats.GetShootDelay())
        {
            attackTimer = 0;
            //ShootingPatterns.ShootTowardsTarget(machine.target, machine.shootPoint, Shoot, 3, 16f);
            ShootingPatterns.ShootAround(Shoot, 4);
        }
    }

    public void FixedUpdate()
    {

    }

    void Shoot(float angle)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        GameObject projectileObj = ObjectPooler.Instance.SpawnFromPool(Machine.ProjectilePoolName, Machine.ShootPoint.position, rotation);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        projectile.Init(Machine.Stats.GetShotSpeed(), Machine.Stats.GetDamage());
    }


}
