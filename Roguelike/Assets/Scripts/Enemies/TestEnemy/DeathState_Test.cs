
using UnityEngine;

public class DeathState_Test : IState
{
    public EnemyStateMachine Machine { get; set; }

    float fadeTimer;
    float fadeTime;

    public DeathState_Test(EnemyStateMachine m)
    {
        Machine = m;
    }

    public void Start()
    {
        fadeTime = 2f;
        fadeTimer = 0;
    }

    public void Update()
    {
        Fade();

        if(fadeTimer > fadeTime)
        {
            OnDeath();
            Machine.gameObject.SetActive(false);
        }
    }

    public void FixedUpdate()
    {

    }

    void Fade()
    {
        fadeTimer += Time.deltaTime;
        float t = fadeTimer / fadeTime;
        float alpha = Mathf.Lerp(1f, 0f, t);
        Machine.SpriteRenderer.color = new Color(Machine.SpriteRenderer.color.r, Machine.SpriteRenderer.color.g, Machine.SpriteRenderer.color.b, alpha);
    }

    void OnDeath()
    {
        Debug.Log("Kuolin :D");
    }


}
