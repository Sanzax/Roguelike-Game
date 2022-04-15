using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    EnemyStateMachine Machine { get; set; }

    void Start();
    void Update();
    void FixedUpdate();
}
