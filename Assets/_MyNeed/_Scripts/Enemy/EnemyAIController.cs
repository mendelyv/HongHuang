//AI v2.0
//AI效果如下：

//1.将怪物分为以下几种状态：
//待机状态（该状态有两种行为：原地呼吸，和游走，可通过权重配置待机状态下的两种行为的发生的比例）
//警戒状态（怪物向玩家发出警告，并盯着玩家）
//追击状态（怪物跑向玩家）
//返回状态（跑回出生位置，该状态下不再理会玩家）

//2.可以为怪物配置各项状态的数值
//游走半径，待机状态下，怪物游走时不会超过这个范围，根据出生点计算
//警戒半径，当玩家与怪物之间距离小于警戒半径时进入警戒状态，根据怪物实时位置计算
//自卫半径，当玩家与怪物之间距离小于自卫半径时进入追击状态
//追击半径，怪物追击玩家时，自身不会超过这个范围，超出后跑回出生点，可以理解为最大活动范围
//攻击距离，当玩家与怪物之间的距离小于攻击距离时攻击

//3.各项数值设定的关系如下
//比较合理的关系是：追击半径 > 警戒半径 > 自卫半径 > 攻击距离，游走半径只需要小于追击半径即可
// 1）自卫半径不建议大于警戒半径，否则就无法触发警戒状态，直接开始追击了
// 2）攻击距离不建议大于自卫半径，否则就无法触发追击状态，直接开始战斗了
// 3）游走半径不建议大于追击半径，否则怪物可能刚刚开始追击就返回出生点


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{

    private GameObject player;//获取玩家单位
    private Animator anim;
    private NavMeshAgent nav;
    private Vector3 initialPosition;//出生位置
    private Vector3[] wanderPositions;//游走的随机存放点
    private Vector3 wanderPosition;
    private PlayerInfo playerInfoScript;
    private EnemyInfo info;
    private EnemyAttack attackScript;

    public Dictionary<string, int> animID;//存放动画状态的字典

    [Header("== 属性值（要在EnemyInfo中获取） ==")]
    public float wanderRadius;//游走半径，移动状态下，如果超出游走半径会返回出生位置
    public float alertRadius;//警戒半径，玩家进入后怪物会发出警告，并一直朝向玩家
    public float defendRadius;//自卫半径，玩家进入后怪物会追击玩家，当距离<攻击距离则会发动攻击
    public float chaseRadius;//追击半径，当怪物超出这个追击半径后放弃追击，返回初始位置
    public float attackDistance;//攻击距离
    public float walkSpeed;
    public float runSpeed;
    public float turnSpeed;//转身速度
    public float nextActBetweenTime;//更换待机指令的间隔时间
    public float[] actionWeight = { 3000, 5000 };

    public enum EnemyState
    {

        IDLE,//待机
        WALK,//移动
        WARN,//盯着玩家
        CHASE,//追击玩家
        ATTACK,//攻击玩家
        RETURN,//超出追击范围返回      

    }

    private EnemyState currentState = EnemyState.IDLE;//默认为原地呼吸

    private float distance_Player;//怪物与玩家的距离
    private float distance_Initial;//怪物与初始位置的距离
    private Quaternion playerRotation;//怪物的目标朝向

    private bool isWarned = false;
    private bool isRunning = false;

    private float actionTimer;//动作计时器
    private float attackTimer;//攻击计时器


    void Start()
    {
        animID = new Dictionary<string, int>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();

        if(player)
            playerInfoScript = player.GetComponent<PlayerInfo>();

        info = GetComponent<EnemyInfo>();
        attackScript = GetComponent<EnemyAttack>();

        //保存初始位置信息
        initialPosition = transform.position;

        //生成时给赋值游走的坐标点（前后左右四个点）
        wanderPositions = new Vector3[4];
        wanderPositions[0] = new Vector3(initialPosition.x + wanderRadius / 2 + Random.Range(0, 5.0f), initialPosition.y, initialPosition.z);
        wanderPositions[1] = new Vector3(initialPosition.x + wanderRadius / 2 - Random.Range(0, 5.0f), initialPosition.y, initialPosition.z);
        wanderPositions[2] = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z + wanderRadius / 2 + Random.Range(0, 5.0f));
        wanderPositions[3] = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z + wanderRadius / 2 - Random.Range(0, 5.0f));

        attackDistance = info.attackDistance;
        nav.stoppingDistance = attackDistance - 0.2f;

        //分配动画ID
        animID.Add("Idle", 0);
        animID.Add("Walk", 1);
        animID.Add("Run", 2);
        animID.Add("Warn", 3);
        animID.Add("Attack", 4);
        animID.Add("Wound", 5);

        //给每一个怪物配置死亡掉落的经验和金币
        info.golden = Random.Range(50, 70);
        info.exp = Random.Range(60, 75);

        //应当检测一下长度做保护···

        //随机一个待机动作
        RandomAction();
    }

    //根据权重随机待机指令
    void RandomAction()
    {

        //根据权重随机
        float number = Random.Range(0, actionWeight[0] + actionWeight[1]);

        //根据权重随机到站立待机状态
        if (number <= actionWeight[0])
        {
            currentState = EnemyState.IDLE;
        }
        //根据权重随机到游走状态
        else if (actionWeight[0] <= number && number <= actionWeight[0] + actionWeight[1])
        {
            currentState = EnemyState.WALK;

            wanderPosition = wanderPositions[(int)Random.Range(0, 3)];
            nav.SetDestination(wanderPosition);
        }

    }

    void Update()
    {

        //如果检测到自己已经死了
        if (info.isDead)
        {
            PlayerAward();
            player.GetComponent<PlayerAttack>().mouseTarget = null;
            attackScript.enabled = false;
            nav.enabled = false;
            GetComponent<CharacterController>().enabled = false;
            Destroy(this.gameObject, 2.0f);
            this.enabled = false;
            return;
        }

        actionTimer += Time.deltaTime;
        switch (currentState)
        {
            //待机状态，等待nextActBetweenTime后重新随机指令
            case EnemyState.IDLE:
                anim.SetInteger("State", animID["Idle"]);
                anim.SetBool("canAttack", false);

                //导航在站立状态下要停止
                if (!nav.isStopped)
                    nav.Stop();
                if (actionTimer > nextActBetweenTime)//时间到再一次思考
                {
                    actionTimer = 0.0f;//更新计时器时间
                    RandomAction();//随机切换指令，随机思考干什么
                }
                //该状态下的检测指令
                IdleCheck();
                break;

            //游走状态，根据状态随机时生成的目标位置修改朝向，并向前移动
            case EnemyState.WALK:
                anim.SetInteger("State", animID["Walk"]);

                //要行走开启导航
                if (nav.isStopped)
                    nav.isStopped = false;
                nav.speed = walkSpeed;

                if (actionTimer > nextActBetweenTime)//时间到再一次思考
                {
                    actionTimer = 0.0f;//更新计时器时间
                    RandomAction();//随机切换指令，随机思考干什么
                }
                //该状态下的检测指令
                WalkCheck();
                break;

            //警戒状态，播放一次动画和声音，并持续朝向玩家位置
            case EnemyState.WARN:
                if (!isWarned)
                {
                    //警戒状态下导航停止
                    nav.Stop();
                    anim.SetInteger("State", animID["Warn"]);
                    isWarned = true;
                }

                //持续朝向玩家位置
                playerRotation = Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, turnSpeed);

                //该状态下检测指令
                WarnCheck();
                break;

            //追击状态，朝着玩家跑去
            case EnemyState.CHASE:
                if (!isRunning)
                {
                    anim.SetInteger("State", animID["Run"]);
                    anim.SetBool("canAttack", false);
                    isRunning = true;
                }
                if (nav.isStopped)
                    nav.isStopped = false;
                nav.speed = runSpeed;
                nav.SetDestination(player.transform.position);

                //持续朝向玩家位置
                playerRotation = Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, playerRotation, turnSpeed);
                //该状态下的检测指令
                ChaseCheck();
                break;

            case EnemyState.ATTACK:
                //攻击动画播放由攻击脚本控制，伤害函数由动画帧调用
                if (!attackScript.canAttackPlayer)
                    attackScript.canAttackPlayer = true;

                AttackCheck();
                break;

            //返回状态，超出追击范围后返回出生位置
            case EnemyState.RETURN:
                //朝向初始位置移动
                if (nav.isStopped)
                    nav.isStopped = false;
                nav.speed = walkSpeed;
                nav.SetDestination(initialPosition);

                //播放步行动画
                anim.SetInteger("State", animID["Walk"]);

                //该状态下的检测指令
                ReturnCheck();
                break;
        }


    }

    //原地呼吸，观察状态的检测，用于切换其他状态
    void IdleCheck()
    {
        if (!player)
            return;

        distance_Player = Vector3.Distance(player.transform.position, transform.position);

        //如果距离可以攻击
        if (distance_Player < attackDistance)
        {
            if (!IsPlayerDead())
            {
                //战斗·····
                currentState = EnemyState.ATTACK;
                attackScript.canAttackPlayer = true;
                isRunning = false;
                isWarned = false;
            }
        }

            //如果进入自卫半径
        else if (distance_Player < defendRadius)
        {
            currentState = EnemyState.CHASE;
        }

            //如果进入警告半径
        else if (distance_Player < alertRadius)
        {
            currentState = EnemyState.WARN;
        }
    }


    //游走状态检测，检测敌人距离及游走是否越界
    void WalkCheck()
    {
        if (!player)
            return;

        distance_Player = Vector3.Distance(player.transform.position, transform.position);
        distance_Initial = Vector3.Distance(transform.position, initialPosition);

        //如果距离可以攻击
        if (distance_Player < attackDistance)
        {
            if (!IsPlayerDead())
            {
                //战斗·····
                currentState = EnemyState.ATTACK;
                attackScript.canAttackPlayer = true;
                isRunning = false;
                isWarned = false;
            }
        }

            //如果进入自卫半径，改为追击状态
        else if (distance_Player < defendRadius)
        {
            currentState = EnemyState.CHASE;
        }
        //如果进入警戒半径，改为警戒状态
        else if (distance_Player < alertRadius)
        {
            currentState = EnemyState.WARN;
        }

        //如果超出活动半径，就调整朝向为初始点
        if (distance_Initial > wanderRadius)
        {
            currentState = EnemyState.WALK;
            nav.SetDestination(initialPosition);
        }

        //如果与wanderPoint距离满足，就由WALK切换为IDLE
        float distance_Wander = Vector3.Distance(transform.position, wanderPosition);
        if (distance_Wander <= nav.stoppingDistance)
        {
            currentState = EnemyState.IDLE;
        }


    }


    //警告状态下的检测，用于取消追击及取消警戒状态
    void WarnCheck()
    {
        if (!player)
            return;

        distance_Player = Vector3.Distance(player.transform.position, transform.position);

        //如果进入自卫半径，取消警戒状态，进入追击状态
        if (distance_Player < defendRadius)
        {
            isWarned = false;
            currentState = EnemyState.CHASE;
        }

        //如果超出警戒半径，取消警戒状态，然后随机思考待机指令
        if (distance_Player > alertRadius)
        {
            isWarned = false;
            RandomAction();

        }
    }


    //追击状态检测，检测敌人是否进入攻击范围以及是否离开警戒范围
    void ChaseCheck()
    {
        if (!player)
            return;

        distance_Player = Vector3.Distance(player.transform.position, transform.position);
        distance_Initial = Vector3.Distance(transform.position, initialPosition);

        //如果距离可以攻击
        if (distance_Player < attackDistance)
        {
            if (!IsPlayerDead())
            {
                //战斗·····
                currentState = EnemyState.ATTACK;
                attackScript.canAttackPlayer = true;
                isRunning = false;
                isWarned = false;
            }
            else
            {
                currentState = EnemyState.IDLE;
            }
        }

        //如果超出追击范围或者敌人的距离超出警戒距离就返回
        if (distance_Initial > chaseRadius || distance_Player > alertRadius)
        {
            currentState = EnemyState.RETURN;
        }
    }

    //攻击状态下的检测
    public void AttackCheck()
    {
        if (!player)
            return;

        distance_Player = Vector3.Distance(player.transform.position, transform.position);
        distance_Initial = Vector3.Distance(transform.position, initialPosition);

        if (PlayerStaticInfo.Instance.currentHP <= 0)
        {
            currentState = EnemyState.IDLE;
            attackScript.canAttackPlayer = false;
        }
        if (distance_Player > attackDistance)
        {
            currentState = EnemyState.CHASE;
            attackScript.canAttackPlayer = false;
        }
        if (distance_Initial > chaseRadius)
        {
            currentState = EnemyState.RETURN;
        }
    }

    //超出追击半径，返回状态的检测，不再检测敌人距离
    void ReturnCheck()
    {
        if (!player)
            return;

        distance_Initial = Vector3.Distance(transform.position, initialPosition);
        distance_Player = Vector3.Distance(transform.position, player.transform.position);

        //如果已经接近初始位置，则随机一个待机状态
        if (distance_Initial <= nav.stoppingDistance)
        {
            isRunning = false;
            RandomAction();
        }
    }

    public bool IsPlayerDead()
    {
        if (PlayerStaticInfo.Instance.currentHP <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    
    //怪物掉落
    public void PlayerAward()
    {
        PlayerStaticInfo.Instance.currentExp += info.exp;
        PlayerStaticInfo.Instance.golden += info.golden;
        PlayerStaticInfo.Instance.LevelUp();
    }

}
