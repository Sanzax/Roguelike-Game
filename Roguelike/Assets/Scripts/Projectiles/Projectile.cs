using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour, IPooledObject
{
    [SerializeField]
    protected Rigidbody rb;

    protected float moveSpeed;
    protected float damage;

    [SerializeField]
    protected float maxLifeTime;

    protected float lifeTime;

    public void OnObjectSpawn()
    {
        lifeTime = maxLifeTime;
    }

    void Update()
    {
        ReduceLifeTime();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        rb.velocity = transform.right * moveSpeed;
    }

    void ReduceLifeTime()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void Init(float s, float d)
    {
        moveSpeed = s;
        damage = d;
    }
}
