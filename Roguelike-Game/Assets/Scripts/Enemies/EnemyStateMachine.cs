using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    protected IState[] states;
    protected IState currentState;

    [SerializeField] PathFindingUnit pathUnit;
    public PathFindingUnit PathUnit { get { return pathUnit; } }

    [SerializeField] SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer { get { return spriteRenderer; } }

    [SerializeField] EnemyStatCalculation stats;
    public EnemyStatCalculation Stats { get { return stats; } }

    [SerializeField] Transform shootPoint;
    public Transform ShootPoint { get { return shootPoint; } }

    public GameObject Player { get; private set; }
    public Transform Target { get; set; }

    [SerializeField] string projectilePoolName;
    public string ProjectilePoolName { get { return projectilePoolName; } }

    protected void Init()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Target = Player.transform;
        PathUnit.Speed = stats.GetSpeed();
    }

    public void ChangeState(int state)
    {
        currentState = states[state];
        currentState.Start();
    }

    void Update()
    {
        currentState.Update();
    }

    void FixedUpdate()
    {
        currentState.FixedUpdate();
    }
}
