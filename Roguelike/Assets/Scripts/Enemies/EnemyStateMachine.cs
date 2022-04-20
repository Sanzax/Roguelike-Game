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
    public Vector3 prevTargetPosition { get; set; }

    [SerializeField] string projectilePoolName;
    public string ProjectilePoolName { get { return projectilePoolName; } }

    public float distanceFromPlayer = 6 * 6;

    public float AttackTimer { get; set; }

    [SerializeField] bool isActive;
    public bool IsActive { get { return isActive; } set { isActive = value; } }

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
        if(IsActive)
        {
            AttackTimer += Time.deltaTime;
            currentState.Update();
            prevTargetPosition = Target.position;
        }
    }

    void FixedUpdate()
    {
        if(IsActive)
        {
            currentState.FixedUpdate();
        }
    }


    public void Shoot(float angle)
    {
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        GameObject projectileObj = ObjectPooler.Instance.SpawnFromPool(ProjectilePoolName, ShootPoint.position, rotation);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        projectile.Init(Stats.GetShotSpeed(), Stats.GetDamage());
    }

    public void KeepDistanceFromPlayer()
    {
        if (Vector3.Distance(Player.transform.position, transform.position) > distanceFromPlayer)
        {
            if (!PathUnit.IsMoving)
            {
                PathUnit.MoveTo(Target.position);
                Debug.Log("Uus reitti, olin paikoillani");
            }
            else if (Target.position != prevTargetPosition)
            {
                PathUnit.MoveTo(Player.transform.position);
                Debug.Log("Uus reitti, olin jo liikeessä");
            }
        }
        else
        {
            PathUnit.Stop();
        }
    }

    public bool IsTargetInLineOfSight()
    {
        return true;
    }

    public bool CanAttack()
    {
        if (AttackTimer > Stats.GetShootDelay())
        {
            return true;
        }
        return false;
    }

}
