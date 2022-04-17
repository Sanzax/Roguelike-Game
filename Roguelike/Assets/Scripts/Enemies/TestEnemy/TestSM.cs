using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TestState
{
    idle,
    attack,
    death
}

public class TestSM : EnemyStateMachine
{
    AttackState_Test attackState;
    IdleState_Test idleState;
    DeathState_Test deathState;

    void Awake()
    {
        Init();
        attackState = new AttackState_Test(this);
        idleState = new IdleState_Test(this);
        deathState = new DeathState_Test(this);

        states = new IState[] { idleState, attackState, deathState };

        ChangeState((int)TestState.attack);
    }

}