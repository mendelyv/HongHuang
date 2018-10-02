using UnityEngine;
using System.Collections;

public class PlayerInfo : BaseInfo
{
    void Start()
    {
        InitMember();
    }

    public override void InitMember()
    {
        base.InitMember();
    }

    public override void GetDamage(int damage)
    {
        damage -= def;
        PlayerStaticInfo.Instance.currentHP -= damage;
        anim.SetTrigger("Wound");
        IsDead();
    }

    public override void IsDead()
    {
        if(PlayerStaticInfo.Instance.currentHP <= 0)
        {
            PlayerStaticInfo.Instance.currentHP = 0;
            isDead = true;
            nav.enabled = false;
            anim.SetBool("isDead", true);
        }
    }

}
