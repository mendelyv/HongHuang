using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterChoosePanel : MonoBehaviour {

    public Transform characterToggle;

    [Header("== 创建角色的模特 ==")]
    public GameObject humonModel;
    public GameObject wizardsModel;
    public GameObject demonModel;

    private int type = -1;
    [HideInInspector]
    public string name;

    void Update()
    {
        //遍历所有toggle
        for(int i = 0; i < characterToggle.childCount; i++)
        {
            Transform tmp = characterToggle.GetChild(i);
            //如果有toggle勾选
            if(tmp.GetComponent<Toggle>().isOn)
            {
                name = tmp.transform.Find("CharacterName").GetComponent<Text>().text;
                if(tmp.transform.Find("CharacterType").GetComponent<Text>().text.Equals("人族"))
                {
                    type = (int)PlayerType.HUMAN;
                }
                else if(tmp.transform.Find("CharacterType").GetComponent<Text>().text.Equals("仙族"))
                {
                    //给需要创建的人物模型赋值
                    //GameManager.Instance.playerObj = Resources.Load<GameObject>("Prefabs/Player/Player_YangJian");
                    GameManager.Instance.type = (PlayerType)type;
                    type = (int)PlayerType.WIZARDS;
                }
                else if(tmp.transform.Find("CharacterType").GetComponent<Text>().text.Equals("妖族"))
                {
                    //给需要创建的人物模型赋值
                    //GameManager.Instance.playerObj = Resources.Load<GameObject>("Prefabs/Player/Player_BaiGuNiangNiang");
                    GameManager.Instance.type = (PlayerType)type;
                    type = (int)PlayerType.DEMON;
                }

                break;
            }
        }

        switch(type)
        {
            case (int)PlayerType.HUMAN:
                humonModel.SetActive(true);
                wizardsModel.SetActive(false);
                demonModel.SetActive(false);
                break;

            case (int)PlayerType.WIZARDS:
                humonModel.SetActive(false);
                wizardsModel.SetActive(true);
                demonModel.SetActive(false);
                break;

            case (int)PlayerType.DEMON:
                humonModel.SetActive(false);
                wizardsModel.SetActive(false);
                demonModel.SetActive(true);
                break;
        }
        
    }

   


}
