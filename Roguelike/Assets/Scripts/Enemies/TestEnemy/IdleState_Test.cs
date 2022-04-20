
using UnityEngine;

public class IdleState_Test : IState
{
    public EnemyStateMachine Machine { get; set; }

    public IdleState_Test(EnemyStateMachine m)
    {
        Machine = m;
    }

    public void Start()
    {

    }

    public void Update()
    {
        Debug.Log("Idle");
    }

    public void FixedUpdate()
    {

    }


}
