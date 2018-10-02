using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;


public enum ItemType
{

    WEAPON,//武器
    COAT,//上衣
    TROUSERS,//裤子
    SHOES,//鞋
    CUFF,//护腕
    BELT,//腰带
    RING_1,//戒指
    RING_2,//戒指

}

public struct SItem
{

    public int bagID;//背包编号
    public string name;
    public string iconName;//物品图标的名称
    public string description;//物品的描述
    public int hp;
    public int mp;
    public int atk;
    public int def;
    public int num;
    public int canOverly;//装备是否可以叠加，0为不可以，1为可以
    
}

public class ItemManager  {

    private static ItemManager instance;
    public static ItemManager Instance
    {
        get
        {
            if (instance == null)
                instance = new ItemManager();

            return instance;
        }
    }

    public Dictionary<string, SItem> allItemDic;

    //从外部文件导入所有装备信息
    public void Init(string path)
    {
        if(!File.Exists(path))
        {
            Debug.Log("FILE ERROR");
            return;
        }
        allItemDic = new Dictionary<string, SItem>();
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        XmlNode root = doc.SelectSingleNode("Root");
        XmlNode item = root.SelectSingleNode("Item");

        while(item != null)
        {
            SItem temp = new SItem();
            temp.name = item.Attributes["name"].Value;
            temp.iconName = item.Attributes["iconName"].Value;
            temp.description = item.InnerText;
            temp.hp = int.Parse(item.Attributes["hp"].Value);
            temp.mp = int.Parse(item.Attributes["mp"].Value);
            temp.atk = int.Parse(item.Attributes["atk"].Value);
            temp.def = int.Parse(item.Attributes["def"].Value);
            temp.num = int.Parse(item.Attributes["num"].Value);
            temp.canOverly = int.Parse(item.Attributes["canOverly"].Value);

            allItemDic.Add(temp.name, temp);

            item = item.NextSibling;
        }

    }

}
