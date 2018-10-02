using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAttack : MonoBehaviour
{
    [HideInInspector]
    public bool canAttackPlayer = false;

    private float atkDistance;//攻击距离
    private float atkBetween;//攻击间隔，攻速

    private GameObject target;

    private Animator anim;
    private EnemyAIController enemyAIControllerScript;
    private EnemyInfo info;

    private float attackTimer;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        enemyAIControllerScript = GetComponent<EnemyAIController>();
        info = GetComponent<EnemyInfo>();
        atkDistance = info.attackDistance;
        atkBetween = info.attackBetween;
        attackTimer = atkBetween;
    }

    void Update()
    {
        //如果可以攻击
        if (canAttackPlayer)
        {
            //如果在攻击间隔时间中
            if (attackTimer >= atkBetween)
            {
                Invoke("attackTimerRefresh", 1.0f);//调用重置计时器的函数
                transform.LookAt(target.transform.position);
                anim.SetBool("canAttack", true);
            }
            else//如果不在攻击间隔中
            {
                attackTimer += Time.deltaTime;
                //回到Idle动画状态
                anim.SetBool("canAttack", false);
                anim.SetInteger("State", 0);
            }
        }
        else//如果不可以攻击
        {
            anim.SetBool("canAttack", false);
            attackTimer = atkBetween;//重置计时器避免下次接近时等待
        }



    }


    //重置计时器的时间，在攻击动画播放完毕后
    private void attackTimerRefresh()
    {
        attackTimer = 0.0f;
    }

    //这个函数在动画帧时间中调用
    public void Attack()
    {
        int damage = info.atk;
        target.GetComponent<PlayerInfo>().GetDamage(damage);
    }



}



