using UnityEngine;
using System.Collections;

public class EnemyInfo : BaseInfo
{
    void Start()
    {
        InitMember();
    }

    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);

        IsDead();
    }

}
