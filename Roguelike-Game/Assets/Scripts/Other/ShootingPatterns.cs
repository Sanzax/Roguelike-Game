using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ShootingPatterns
{
    public static void ShootTowardsTarget(Vector3 target, Vector3 self, Action<float> shootFunc, int amount = 1, float spread = 0f)
    {
        Vector3 direction = (target - self).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (amount % 2 == 0)
            angle -= ((amount - 1) / 2) * spread + spread / 2;
        else
            angle -= ((amount - 1) / 2) * spread;

        for (int i = 0; i < amount; i++)
        {
            shootFunc(angle);
            angle += spread;
        }
    }

    public static void ShootAround(Action<float> shootFunc, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            float angle = 360 / amount * i;
            shootFunc(angle);
        }
    }

}
