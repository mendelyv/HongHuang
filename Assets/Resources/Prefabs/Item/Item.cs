using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour {

    private Transform UIRoot;
    private Camera UICamera;
    private Ray ray;
    private RaycastHit rayHit;
    private RectTransform _transform;
    private int rayLayer;
    private float rayLength;

    [HideInInspector]
    public Transform startGrid;
    [HideInInspector]
    public Transform endGrid;

    public int bagID;
    public int clickTimes = 0;


    void Start()
    {
        
        rayLength = 50.0f;
        rayLayer = LayerMask.GetMask("FrameRayCastLayer");
        UIRoot = GameObject.FindGameObjectWithTag("UIRoot").transform;
        startGrid = transform.parent.gameObject.transform;
        UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
        _transform = GetComponent<RectTransform>();
        startGrid = transform.parent;
    }


    //拖动物品
    public void DragItem()
    {
        Vector3 mouseWorld = UICamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0.0f;
        _transform.position = mouseWorld;
        transform.parent = UIRoot;

        ray = new Ray(UICamera.transform.position, mouseWorld - UICamera.transform.position);
        Debug.DrawLine(UICamera.transform.position, mouseWorld, Color.yellow, 0.5f);

        if(Physics.Raycast(ray,out rayHit,rayLength,rayLayer))
        {
                endGrid = rayHit.transform;
        }
        
    }

    //松开物品
    public void DropItem()
    {
        if(endGrid.transform.childCount > 0)
        {
            endGrid.transform.GetChild(0).parent = startGrid.transform;
            startGrid.transform.GetChild(0).transform.localPosition = Vector3.zero;
            startGrid.transform.GetChild(0).transform.GetComponent<Item>().startGrid = startGrid;
        }
        transform.parent = endGrid;
        transform.localPosition = Vector3.zero;
        startGrid = transform.parent.transform;
        clickTimes = 0;
    }

    public void OnMouse_RClick()
    {
        clickTimes++;
        if(clickTimes == 2)
        {
            //使用道具
            Debug.Log("bagID : " + bagID);
            foreach(SItem val in PlayerStaticInfo.Instance.bagList)
            {
                //找到相应背包ID的物品
                if(val.bagID == bagID)
                {
                    
                }
            }
            clickTimes = 0;
        }

    }



}
