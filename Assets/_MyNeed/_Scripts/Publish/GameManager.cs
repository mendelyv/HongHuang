using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager  {

   
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GameManager();
            return instance;
        }
    }

    //账号名字
    public string userName;
    //角色预制体
    public GameObject playerObj;
    //角色职业
    public PlayerType type;
    //是否创建过角色和UI
    public bool isPlayerAndUIBirth = false;

    public void GivePlayerObj()
    {
        switch(type)
        {
            case PlayerType.HUMAN:
                break;

            case PlayerType.WIZARDS:
                GameManager.Instance.playerObj = Resources.Load<GameObject>("Prefabs/Player/Player_YangJian");
                break;

            case PlayerType.DEMON:
                break;
        }
    }
}
