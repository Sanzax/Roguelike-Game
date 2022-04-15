using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatCalculation : MonoBehaviour
{
    [SerializeField]
    PlayerBaseStats baseStats;

    float additionalHp;
    float additionalSpeed;
    float additionalDamage;
    float additionalShootDelay;
    float additionalRange;
    float additionalShotSpeed;
    float additionalLuck;

    float dmgMultiplier = 1f;

    public float GetHp()
    {
        return baseStats.hp + additionalHp;
    }

    public float GetSpeed()
    {
        return baseStats.speed + additionalSpeed;
    }

    public float GetDamage()
    {
        return (baseStats.damage + additionalDamage) * dmgMultiplier;
    }

    public float GetShootDelay()
    {
        return baseStats.shootDelay + additionalShootDelay;
    }

    public float GetRange()
    {
        return baseStats.range + additionalRange;
    }

    public float GetShotSpeed()
    {
        return baseStats.shotSpeed + additionalShotSpeed;
    }

    public float GetLuck()
    {
        return baseStats.luck + additionalLuck;
    }

    public void AddHp(float a)
    {
        additionalHp += a;
    }

    public void AddSpeed(float a)
    {
        additionalSpeed += a;
    }

    public void AddDamage(float a)
    {
        additionalDamage += a;
    }

    public void AddShootDelay(float a)
    {
        additionalShootDelay += a;
    }

    public void AddRange(float a)
    {
        additionalRange += a;
    }

    public void AddShotSpeed(float a)
    {
        additionalShotSpeed += a;
    }

    public void AddLuck(float a)
    {
        additionalLuck += a;
    }

    public void SetDamageMultiplier(float m)
    {
        dmgMultiplier = m;
    }
}
