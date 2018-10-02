using UnityEngine;
using System.Collections;

public class RadiusTest : MonoBehaviour {

    public float wanderRadius;//游走半径，移动状态下，如果超出游走半径会返回出生位置
    public float alertRadius;//警戒半径，玩家进入后怪物会发出警告，并一直朝向玩家
    public float defendRadius;//自卫半径，玩家进入后怪物会追击玩家，当距离<攻击距离则会发动攻击
    public float chaseRadius;//追击半径，当怪物超出这个追击半径后放弃追击，返回初始位置
    public float attackRange;//攻击距离

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, alertRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, defendRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, attackRange);

    }

}
