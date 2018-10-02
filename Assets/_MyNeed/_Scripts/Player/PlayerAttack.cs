using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    [HideInInspector]
    public GameObject mouseTarget;//这里的mouseTarget即为PlayerController脚本中的

    private bool canAttack = false;
    private Animator anim;
    private AudioSource audio;
    

    void Start()
    {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        CanAttack();
        if (canAttack)
        {
            transform.LookAt(mouseTarget.transform.position);
            anim.SetBool("canAttack", canAttack);
        }
        else
        {
            anim.SetBool("canAttack", canAttack);
        }
    }

    public void CanAttack()
    {
        if (mouseTarget == null)
        {
            canAttack = false;
            return;
        }

        float distance = Vector3.Distance(transform.position, mouseTarget.transform.position);
        if (distance <= PlayerStaticInfo.Instance.attackDistance)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
    }

    public void Attack()
    {
        if (mouseTarget == null)
            return;

        int damage = PlayerStaticInfo.Instance.atk;
        mouseTarget.GetComponent<EnemyInfo>().GetDamage(damage);
        audio.Play();
    }

}
