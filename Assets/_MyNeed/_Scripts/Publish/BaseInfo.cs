//属性信息基类，用于怪物跟人物属性


using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class BaseInfo : MonoBehaviour
{

    public string name;
    public int maxHP;
    public int currentHP;
    public int atk;
    public int def;
    public int golden;//金钱，人物身上为所剩金币，怪物身上为掉落金币
    public int exp;//经验
    public float attackDistance;//攻击距离
    public float attackBetween;//攻击间隔

    public bool isDead = false;

    protected Animator anim;
    protected NavMeshAgent nav;

    //初始化该类所有成员信息
    public virtual void InitMember()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }

    //受到伤害
    public virtual void GetDamage(int damage)
    {
        currentHP -= damage;
        anim.SetTrigger("Wound");

    }

    //判断是否死亡
    public virtual void IsDead()
    {
        if (currentHP <= 0)
        {
            currentHP = 0;
            isDead = true;
            nav.enabled = false;
            anim.SetBool("isDead", isDead);
        }
    }



}
