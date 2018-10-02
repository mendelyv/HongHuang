using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

public enum PlayerType
{
    HUMAN,//人族  0
    WIZARDS,//仙族  1
    DEMON,//妖族  2
}

public class PlayerStaticInfo  {

    private static PlayerStaticInfo instance;
    public static PlayerStaticInfo Instance
    {
        get
        {
            if (instance == null)
                instance = new PlayerStaticInfo();
            return instance;
        }
    }


    //基础数据层
    public string name;
    public PlayerType type;
    public int maxHP;
    public int currentHP;
    public int maxMP;
    public int currentMP;
    public int maxExp;
    public int currentExp;
    public int atk;
    public int def;
    public int level;
    public int golden;//金钱，人物身上为所剩金币，怪物身上为掉落金币
    public float attackDistance;//攻击距离
    public float attackBetween;//攻击间隔
    public int HPrecoverAmount;//每秒钟增加的血量
    public int MPrecoverAmount;//每秒钟增加的蓝量

    public string sceneName;//角色所在场景名
    public Vector3 scenePos;//角色所在场景的位置

    public float levelUpProprety = 0.2f;//升级增加的属性百分比

    //背包容器
    public List<SItem> bagList;

    //装备容器
    public Dictionary<ItemType, SItem> equipsDic;


    public void Init(string characterName)
    {

        //需要一个角色的名字来知道是哪个角色
        string path = Application.streamingAssetsPath + "/CharacterInfo.xml";
        if (!File.Exists(path))
        {
            Debug.Log("ERROR");
            return;
        }
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        XmlNode root = doc.SelectSingleNode("Root");
        XmlNode account = root.SelectSingleNode("Account");
        while (account != null)
        {
            //如果账号匹配
            if (account.Attributes["userName"].Value.Equals(GameManager.Instance.userName))
            {
                XmlNode character = account.SelectSingleNode("Character");
                while (character != null)
                {
                    //如果角色名字匹配
                    if (character.Attributes["name"].Value.Equals(characterName))
                    {
                        PlayerStaticInfo.Instance.name = character.Attributes["name"].Value;
                        PlayerStaticInfo.Instance.type = (PlayerType)int.Parse(character.Attributes["type"].Value);
                        PlayerStaticInfo.Instance.maxHP = int.Parse(character.Attributes["maxHP"].Value);
                        PlayerStaticInfo.Instance.currentHP = int.Parse(character.Attributes["currentHP"].Value);
                        PlayerStaticInfo.Instance.maxMP = int.Parse(character.Attributes["maxMP"].Value);
                        PlayerStaticInfo.Instance.currentMP = int.Parse(character.Attributes["currentMP"].Value);
                        PlayerStaticInfo.Instance.maxExp = int.Parse(character.Attributes["maxExp"].Value);
                        PlayerStaticInfo.Instance.currentExp = int.Parse(character.Attributes["currentExp"].Value);
                        PlayerStaticInfo.Instance.atk = int.Parse(character.Attributes["atk"].Value);
                        PlayerStaticInfo.Instance.def = int.Parse(character.Attributes["def"].Value);
                        PlayerStaticInfo.Instance.level = int.Parse(character.Attributes["level"].Value);
                        PlayerStaticInfo.Instance.golden = int.Parse(character.Attributes["golden"].Value);
                        PlayerStaticInfo.Instance.attackDistance = int.Parse(character.Attributes["attackDistance"].Value);
                        PlayerStaticInfo.Instance.attackBetween = int.Parse(character.Attributes["attackBetween"].Value);
                        PlayerStaticInfo.Instance.HPrecoverAmount = int.Parse(character.Attributes["HPrecoverAmount"].Value);
                        PlayerStaticInfo.Instance.MPrecoverAmount = int.Parse(character.Attributes["MPrecoverAmount"].Value);
                        PlayerStaticInfo.Instance.sceneName = character.Attributes["sceneName"].Value;
                        PlayerStaticInfo.Instance.scenePos.x = float.Parse(character.Attributes["sceneXPos"].Value);
                        PlayerStaticInfo.Instance.scenePos.y = float.Parse(character.Attributes["sceneYPos"].Value);
                        PlayerStaticInfo.Instance.scenePos.z = float.Parse(character.Attributes["sceneZPos"].Value);

                        //匹配到角色信息后跳出
                        break;
                    }
                    character = character.NextSibling;
                }
                //匹配到账号后跳出
                break;
            }
            account = account.NextSibling;

        }



    }


    //从xml文件中给玩家背包导入信息
    public void InitBag(string characterName)
    {
        //给玩家背包列表申请内存
        bagList = new List<SItem>();

        //根据账号的信息查看背包的储存，这里先做测试
        string path = Application.streamingAssetsPath + "/CharacterItem.xml";
        if (!File.Exists(path))
        {
            Debug.Log("ERROR");
            return;
        }

        XmlDocument doc = new XmlDocument();
        doc.Load(path);//文件要保存成UTF-8编码
        XmlNode root = doc.SelectSingleNode("Bag");
        XmlNode character = root.SelectSingleNode("Character");
        while(character != null)
        {
            if(character.Attributes["name"].Value.Equals(characterName))
            {
                XmlNode item = character.SelectSingleNode("Item");

                int _bagID = 0;
                while (item != null)
                {
                    string itemName = item.Attributes["name"].Value;
                    //根据名字从所有道具的容器中找到数据
                    SItem itemStruct = ItemManager.Instance.allItemDic[itemName];
                    //在ItemManager中已经为其他属性赋值完毕，这里只需赋值数量和背包所在编号就好
                    itemStruct.num = int.Parse(item.Attributes["num"].Value);
                    itemStruct.bagID = _bagID;

                    //如果这个物品不可以叠加
                    if (itemStruct.canOverly == 0)
                    {

                        for (int i = 0; i < itemStruct.num; i++)
                        {
                            SItem tmp = itemStruct;
                            tmp.num = 1;
                            tmp.bagID = _bagID;
                            _bagID++;
                            //添加进玩家背包链表
                            PlayerStaticInfo.Instance.bagList.Add(tmp);
                        }
                    }
                    else
                    {
                        //添加进玩家背包链表
                        PlayerStaticInfo.Instance.bagList.Add(itemStruct);
                    }

                    item = item.NextSibling;
                    _bagID++;
                }

                break;
            }

            character = character.NextSibling;
        }

    }

    //升级
    public void LevelUp()
    {
        if(currentExp >= maxExp)
        {
            currentExp -= maxExp;
            maxExp = (int)(maxExp + maxExp * levelUpProprety);
            maxHP = (int)(maxHP + maxHP * levelUpProprety);
            maxMP = (int)(maxMP + maxMP * levelUpProprety);
            atk = (int)(atk + atk * levelUpProprety);
            def = (int)(def + def * levelUpProprety);
            level += 1;
        }
    }


}
