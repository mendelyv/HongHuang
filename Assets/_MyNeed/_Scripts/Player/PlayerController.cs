
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{

    public float speed = 6.0f;//人物移动速度
    public Vector3 targetPosition;//鼠标点击地板的点
    public Dictionary<string, int> animID;//存放动画状态的字典

    public bool isFrameOpen = false;//鼠标是否进入UI操作框

    private GameObject mouseArrowPrefab;//鼠标点击地板出现的指示
    private GameObject mouseTarget;//储存鼠标左键最近点击的物体
    private int rayCastLayer;//用层来过滤一些不需要接收射线的物体
    private float camRayLength = 100.0f;//摄像机射线的长度
    public bool isMoving = false;

    //自身组件
    private PlayerInfo info;
    private PlayerAttack attackScript;
    private Rigidbody playerRigidbody;
    private CharacterController characterController;
    private NavMeshAgent nav;
    private Animator anim;

    //辅助射线
    private Ray camRay;
    private RaycastHit camRayHit;

    private float recoverTimer;

    void Awake()
    {
        mouseArrowPrefab = Resources.Load<GameObject>("MouseArrow/Effects_02/Effect_Prefeb/Efx_Click_Orange");

        rayCastLayer = LayerMask.GetMask("RayCastLayer");
        playerRigidbody = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        targetPosition = transform.position;//让开始的移动目标为自己本身
        animID = new Dictionary<string, int>();
        info = GetComponent<PlayerInfo>();
        attackScript = GetComponent<PlayerAttack>();
        nav = GetComponent<NavMeshAgent>();
        mouseTarget = null;

        //分配动画ID
        animID.Add("Idle", 0);
        animID.Add("Run", 1);

    }

    void Update()
    {

        //====================
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(PlayerStaticInfo.Instance.currentHP);
            Debug.Log(PlayerStaticInfo.Instance.maxHP);
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            PlayerStaticInfo.Instance.currentHP -= 20;
        }
        //====================

        
        if(info.isDead)
        {
            anim.SetBool("isDead", true);
            this.enabled = false;
        }

        recoverTimer += Time.deltaTime;
        if(recoverTimer >= 1.0f)
        {
            //回血
            if (PlayerStaticInfo.Instance.currentHP < PlayerStaticInfo.Instance.maxHP)
            {
                recoverTimer = 0.0f;
                PlayerStaticInfo.Instance.currentHP += PlayerStaticInfo.Instance.HPrecoverAmount;
                if (PlayerStaticInfo.Instance.currentHP > PlayerStaticInfo.Instance.maxHP)
                    PlayerStaticInfo.Instance.currentHP = PlayerStaticInfo.Instance.maxHP;
            }

            //回蓝
            if (PlayerStaticInfo.Instance.currentMP < PlayerStaticInfo.Instance.maxMP)
            {
                recoverTimer = 0.0f;
                PlayerStaticInfo.Instance.currentMP += PlayerStaticInfo.Instance.MPrecoverAmount;
                if (PlayerStaticInfo.Instance.currentMP > PlayerStaticInfo.Instance.maxMP)
                    PlayerStaticInfo.Instance.currentMP = PlayerStaticInfo.Instance.maxMP;
            }

        }

        //点击鼠标左键
        if (Input.GetMouseButtonDown(0) && !isFrameOpen)
        {
            CheckPress();//这个函数给mouseTarget赋值
            Debug.DrawLine(camRay.origin, camRayHit.point, Color.red, 2.0f);
        }

        //可以移动
        if (isMoving && !info.isDead)
        {
            if (camRayHit.collider.tag == "Enemy")
            {
                if (nav.isStopped == true)
                    nav.isStopped = false;

                TurnToTargetPosition(mouseTarget.transform.position);
                //这里要使用mouseTarget来给导航赋目标点
                nav.SetDestination(mouseTarget.transform.position);
                anim.SetInteger("State", animID["Run"]);
            }

            //如果鼠标点到NPC
            else if (camRayHit.collider.tag == "NPC")
            {
                if (nav.isStopped == true)
                    nav.isStopped = false;

                TurnToTargetPosition(mouseTarget.transform.position);
                //这里要使用mouseTarget来给导航赋目标点
                nav.SetDestination(mouseTarget.transform.position);
                anim.SetInteger("State", animID["Run"]);
            }

            //如果鼠标点到地板
            else if (camRayHit.collider.tag == "Floor")
            {
                if (nav.isStopped)
                    nav.isStopped = false;

                TurnToTargetPosition(camRayHit.point);
                nav.SetDestination(camRayHit.point);
                anim.SetInteger("State", animID["Run"]);

            }
            //检测是否需要停下来
            if (IsNeedToStop())
            {
                //需要停下来
                isMoving = false;
                nav.isStopped = true;
                anim.SetInteger("State", animID["Idle"]);
            }

        }

    }

    //检测在屏幕上点击时，点击到的目标
    public void CheckPress()
    {

        camRay = Camera.main.ScreenPointToRay(Input.mousePosition);//根据鼠标在屏幕中的位置发射一条射线
        if (Physics.Raycast(camRay, out camRayHit, camRayLength, rayCastLayer))
        {
            //如果点击到的是敌人，就给mouseTarget赋值点击的obj
            if (camRayHit.collider.tag == "Enemy")
            {
                if (camRayHit.collider.GetComponent<EnemyInfo>().isDead)
                {
                    return;
                }
                //给mouseTarget赋值，让代码知晓鼠标点击到的是哪个敌人
                mouseTarget = camRayHit.collider.gameObject;
                attackScript.mouseTarget = mouseTarget;

                nav.stoppingDistance = PlayerStaticInfo.Instance.attackDistance - 0.2f;//如果是敌人就调整导航的停止距离
                nav.speed = speed;

                isMoving = true;
            }

            //如果点击到的是NPC
            else if (camRayHit.collider.tag == "NPC")
            {
                //给mouseTarget赋值，让代码知晓鼠标点击到的是哪个NPC
                mouseTarget = camRayHit.collider.gameObject;

                nav.stoppingDistance = PlayerStaticInfo.Instance.attackDistance - 0.1f;
                nav.speed = speed;

                isMoving = true;
            }

            //如果点击到的是地板就让玩家朝点击方向移动
            else if (camRayHit.collider.tag == "Floor")
            {
                ShowMouseArrow(camRayHit.point);
                mouseTarget = null;
                attackScript.mouseTarget = mouseTarget;

                nav.stoppingDistance = 1.0f;
                nav.speed = speed;

                isMoving = true;
            }
        }
        else
        {
            isMoving = false;
        }


    }


    //实例化移动地点指示箭头
    void ShowMouseArrow(Vector3 point)
    {
        point = new Vector3(point.x, point.y + 0.2f, point.z);
        Instantiate(mouseArrowPrefab, point, Quaternion.identity);
    }

    //转向看着一个点
    void TurnToTargetPosition(Vector3 point)
    {
        targetPosition = new Vector3(point.x, transform.position.y, point.z);
        transform.LookAt(targetPosition);
    }

    #region 使用nav之前的移动方法，忽略不计
    //移动
    //public void Move(Vector3 _target)
    //{

    //    isMoving = true;

    //    //使用NavMeshAgent移动
    //    TurnToTargetPosition(_target);
    //    nav.Resume();
    //    anim.SetInteger("State", animID["Run"]);
    //    nav.SetDestination(_target);


    //    //使用CharacterController组件移动
    //    //float distance = Vector3.Distance(transform.position, targetPosition);
    //    //if (distance > 0.2f)
    //    //{
    //    //    characterController.SimpleMove(transform.forward * speed);
    //    //    anim.SetInteger("State", animID["Run"]);
    //    //}
    //    //else
    //    //{
    //    //    isMoving = false;
    //    //    anim.SetInteger("State", animID["Idle"]);
    //    //}

    //}
    #endregion


    //检测是否需要停下
    public bool IsNeedToStop()
    {
        float distance = 0.0f;

        if (camRayHit.collider.tag == "Floor")
        {
            distance = Vector3.Distance(transform.position, camRayHit.point);
        }
        else if (camRayHit.collider.tag == "Enemy")
        {
            distance = Vector3.Distance(transform.position, mouseTarget.transform.position);
        }
        else if(camRayHit.collider.tag == "NPC")
        {
            distance = Vector3.Distance(transform.position, mouseTarget.transform.position);
        }

        if (distance <= nav.stoppingDistance)
        {
            //需要停下
            return true;
        }
        else
        {
            //不需要停下
            return false;
        }


    }

}

