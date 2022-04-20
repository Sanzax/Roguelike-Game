using UnityEngine;

public class AttackState_Test : IState
{
    public EnemyStateMachine Machine { get; set; }

    public AttackState_Test(EnemyStateMachine m)
    {
        Machine = m;
    }

    public void Start()
    {
        Debug.Log("Hyökkäys tila");
    }

    public void Update()
    {
        Machine.KeepDistanceFromPlayer();
        KeepAttacking();
    }

    public void FixedUpdate()
    {

    }

    void KeepAttacking()
    {
        if(Machine.CanAttack())
        {
            Machine.AttackTimer = 0;
            ShootingPatterns.ShootTowardsTarget(Machine.Target, Machine.transform, Machine.Shoot, 3, 16);
        }
    }


}
