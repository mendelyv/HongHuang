using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml;

public class BagController : MonoBehaviour {

    private Text golden;
    private Button XBtn;
    private Transform[] grids;
    private PlayerController playerControllerComponent;

    void Start()
    {
        golden = transform.Find("Golden").GetComponent<Text>();
        XBtn = transform.Find("XBtn").GetComponent<Button>();
        playerControllerComponent = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        //为grids赋值，方便物品图标创建找寻父节点
        Transform childGrids = transform.Find("Grids").transform;
        grids = new Transform[childGrids.childCount];
        for (int i = 0; i < childGrids.childCount; i++ )
        {
            grids[i] = childGrids.GetChild(i);
        }

        XBtn.onClick.AddListener(OnXBtnClick);
        Init();
    }

    void Update()
    {
        golden.text = PlayerStaticInfo.Instance.golden.ToString();

    }

    //给玩家背包显示上的处理
    public void Init()
    {
        GameObject itemObj = Resources.Load<GameObject>("Prefabs/Item/Item");

        //根据玩家背包链表创建预制体
        foreach(SItem val in PlayerStaticInfo.Instance.bagList)
        {
            GameObject tmp = Instantiate(itemObj);

            Sprite sprite = Resources.Load<Sprite>("ItemIcon/" + val.iconName);
            tmp.GetComponent<Image>().sprite = sprite;
            //给物品的组件中的编号赋值
            tmp.GetComponent<Item>().bagID = val.bagID;
            if (val.num != 1)
                tmp.GetComponentInChildren<Text>().text = val.num.ToString();
            else
                tmp.GetComponentInChildren<Text>().text = "";

            //为物品图标找寻父节点
            for(int i = 0; i < grids.Length; i++)
            {
                if(grids[i].childCount == 0)
                {
                    tmp.transform.SetParent(grids[i]);
                    tmp.transform.localPosition = Vector3.one;
                    tmp.transform.localScale = Vector3.one;
                    //tmp.GetComponent<RectTransform>().localPosition = Vector3.zero;
                    //tmp.GetComponent<RectTransform>().localScale = Vector3.one;
                    break;
                }
            }

        }

    }


    public void OnXBtnClick()
    {
        transform.gameObject.SetActive(false);
        playerControllerComponent.isFrameOpen = false;
    }

    public void OnMouseEnter()
    {
        //鼠标进入之后人物不可以点击地板移动
        //Debug.Log("Mouse in Frame");
        playerControllerComponent.isFrameOpen = true;
    }

    public void OnMouseExit()
    {
        //鼠标退出之后人物可以点击地板移动
        //Debug.Log("Mouse out Frame");
        playerControllerComponent.isFrameOpen = false;
    }


    


}
