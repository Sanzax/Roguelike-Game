
public interface IState
{
    EnemyStateMachine Machine { get; set; }

    void Start();
    void Update();
    void FixedUpdate();
}
