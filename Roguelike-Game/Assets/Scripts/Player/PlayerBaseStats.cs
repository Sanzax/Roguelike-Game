using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/PlayerBaseStats")]
public class PlayerBaseStats : ScriptableObject
{
    public float hp = 100f;

    public float speed = 5f;

    public float damage = 1f;

    public float shootDelay = 0.5f;

    public float range = 5f;

    public float shotSpeed = 5f;

    public float luck = 0f;

}
