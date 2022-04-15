using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ShootingPatterns
{
    public static void ShootTowardsTarget(Transform target, Transform self, Action<float> shootFunc, int amount = 1, float spread = 0f)
    {
        float angle = -Mathf.Atan2(target.position.z - self.position.z, target.position.x - self.position.x) * Mathf.Rad2Deg;

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

    public static void ShootAtAngle(float angle, Action<float> shootFunc, int amount = 1, float spread = 0f)
    {
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
