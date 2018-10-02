
///原脚本摄像机视角BUG：当鼠标在竖直方向猛然拖动时会使摄像机的焦点离开target
///当使用LookAt函数保证焦点不离开时，猛然拖动鼠标又会出现摄像机位置突变
///

using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    [Header("== 相机的最大最小焦距 ==")]
    public float minDistance = 4.0f;
    public float maxDistance = 18.0f;

    private GameObject target;//摄像机看向的目标
    private Vector3 offset;//摄像机距离目标的距离

    private float scrollSpeed = 4.0f;//焦距放大缩小的速度
    private float rotateSpeed = 4.0f;//摄像机旋转的速度
    private float moveSpeed = 4.0f;//摄像机移动的平滑度

    private float distance; //相机的焦距
    private bool canRotate = false;//摄像机是否可以旋转

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("CameraLookAtPoint");
        offset = new Vector3(0.0f, 5.0f, -9.0f);
        transform.position = offset;
        transform.localEulerAngles = new Vector3(15.0f, 0.0f, 0.0f);
    }

    void LateUpdate()
    {
        if(target == null)
        {
            return;
        }
        //镜头的直接移动
        transform.position = target.transform.position + offset;
        //镜头的平滑移动
        //Vector3 camTargetPos = target.position + offset;
        //transform.position = Vector3.Lerp(transform.position, camTargetPos, moveSpeed * Time.deltaTime);
        ScrollView();
        RotateView();
        transform.LookAt(target.transform.position);

    }

    //摄像机焦距缩放
    void ScrollView()
    {
        distance = offset.magnitude;
        distance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
        offset = offset.normalized * distance;
    }

    //摄像机旋转
    void RotateView()
    {
        //按下鼠标右键可以旋转摄像机
        if (Input.GetMouseButtonDown(1))
            canRotate = true;
        if (Input.GetMouseButtonUp(1))
            canRotate = false;

        if (canRotate)
        {
            //防止摄像机在竖直方向的极限角度产生相机位移
            Vector3 OriginalPosition = transform.position;
            Quaternion OriginalRotation = transform.rotation;

            //根据鼠标的水平和竖直偏移量来使相机旋转
            transform.RotateAround(target.transform.position, Vector3.up, Input.GetAxis("Mouse X") * rotateSpeed);
            transform.RotateAround(target.transform.position, transform.right, -Input.GetAxis("Mouse Y") * rotateSpeed);

            float x = transform.eulerAngles.x;
            //控制摄像机在竖直方向上的极限角度
            if (x < 10.0f || x > 80.0f)
            {
                transform.position = OriginalPosition;
                transform.rotation = OriginalRotation;
            }
            x = Mathf.Clamp(x, 20, 80);
            transform.eulerAngles = new Vector3(x, transform.eulerAngles.y, transform.eulerAngles.z);
            offset = transform.position - target.transform.position;
            transform.LookAt(target.transform.position);
        }
    }


}
