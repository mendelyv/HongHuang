using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class SignOnSceneUI : MonoBehaviour {

    private Dictionary<string, string> accountDic;
    private string filePath;
    private string choosedName;

    public GameObject AccountPanel;//账号密码界面
    public GameObject CharacterChoosePanel;//选择角色界面
    public GameObject CreateCharacterPanel;//新建角色界面
    public GameObject alertDialog;//弹框提示
    public GameObject characterModels;

    public Button signOnBtn;//登录按钮
    public Button registerBtn;//注册账号按钮
    public InputField userName;//账号输入框
    public InputField password;//密码输入框
    public GameObject billBoard;//公告板
    public GameObject CharacterChooseBoard;//角色选择框
    public Button CreateNewCharacterButton;//创建新角色按钮
    public Button enterGameBtn;//进入游戏按钮
    public Button ExitGameBtn;//退出游戏按钮
    public Button BackBtn_1;//选择角色界面的返回键
    public Button BackBtn_2;//创建角色界面的返回键




    void Awake()
    {
        //保证该显示的界面显示，不需要显示的关闭
        AccountPanel.SetActive(true);
        CharacterChoosePanel.SetActive(false);
        CreateCharacterPanel.SetActive(false);
        alertDialog.SetActive(false);
        characterModels.SetActive(false);

        accountDic = new Dictionary<string, string>();
        filePath = Application.streamingAssetsPath + "/UserInfo.xml";
    }

    void Start()
    {
        ReadAccountInfoFile();
    }
    
    
    //点击登录按钮回调
    public void OnSignOnBtnClick()
    {
        
        if (userName.text.Equals("") || password.text.Equals(""))
        {
            print("账号或密码为空");
            alertDialog.SetActive(true);
            alertDialog.transform.Find("Text").GetComponent<Text>().text = "账号或密码为空";
            return;
        }

        //跟字典中的账号和密码匹配
        foreach (KeyValuePair<string, string> val in accountDic)
        {
            if (val.Key.Equals(userName.text) && val.Value.Equals(password.text))
            {
                print("登录成功");
                //给游戏管理账号名赋值，方便查看该账号下的角色信息
                GameManager.Instance.userName = userName.text;
                //根据账号查询角色信息
            }
            else
            {
                print("账号或密码错误");
                alertDialog.SetActive(true);
                alertDialog.transform.Find("Text").GetComponent<Text>().text = "账号或密码错误";
                return;
            }
        }

        //隐藏账号密码界面
        AccountPanel.SetActive(false);

        //相机往前平移效果
        Camera.main.GetComponent<Animator>().Play("EnterCharacterChoosePanel");
        
        //显示人物选择面板
        Invoke("OnCameraForward", 1.0f);

    }
    

    //相机前移完成的回调
    public void OnCameraForward()
    {
        CharacterChoosePanel.SetActive(true);
        characterModels.SetActive(true);
        GetAccountCharacterInfo();
    }

    //相机后移完成的回调
    public void OnCameraBack()
    {
        AccountPanel.SetActive(true);
    }

    //点击创建新角色按钮回调
    public void CreateNewCharacterBtnClick()
    {
        CharacterChoosePanel.SetActive(false);
        CreateCharacterPanel.SetActive(true);
        characterModels.SetActive(true);
    }

    public void ExitGameBtnClick()
    {
        Application.Quit();
    }

    //选择角色界面的返回按键回调
    public void BackBtn_1Click()
    {
        CharacterChoosePanel.SetActive(false);
        characterModels.SetActive(false);
        Transform characters = CharacterChoosePanel.transform.Find("CharacterChooseBoard/Characters").transform;


        Camera.main.GetComponent<Animator>().Play("QuitCharacterChoosePanel");
        Invoke("OnCameraBack", 1.0f);
    }

    public void BackBtn_2Click()
    {
        CreateCharacterPanel.SetActive(false);
        CharacterChoosePanel.SetActive(true);
        
        for(int i = 0; i < characterModels.transform.childCount; i++)
        {
            characterModels.transform.GetChild(i).gameObject.SetActive(false);
        }

        characterModels.SetActive(false);
    }

    //从文件中读
    void ReadAccountInfoFile()
    {
        if (!File.Exists(filePath))
        {
            print("File ERROR");
            return;
        }

        XmlDocument doc = new XmlDocument();
        doc.Load(filePath);

        XmlNode root = doc.SelectSingleNode("UserInfo");
        XmlNode node = root.FirstChild;

        while (node != null)
        {
            string userName = node.Attributes["UserName"].Value;
            string password = node.Attributes["Password"].Value;

            accountDic.Add(userName, password);

            node = node.NextSibling;
        }

        print("读取成功");
    }

    //写入文件
    void WriteAccountInfoFile()
    {
        if (userName.text.Equals("") || password.text.Equals(""))
        {
            print("账号或密码为空");
            alertDialog.SetActive(true);
            alertDialog.transform.Find("Text").GetComponent<Text>().text = "账号或密码为空";
            return;
        }

        foreach (KeyValuePair<string, string> val in accountDic)
        {
            if (userName.text.Equals(val.Key))
            {
                print("账号已注册");
                alertDialog.SetActive(true);
                alertDialog.transform.Find("Text").GetComponent<Text>().text = "账号已注册";
                return;
            }
        }

        XmlDocument doc = new XmlDocument();
        XmlElement root;
        bool fileExist = false;

        if (File.Exists(filePath))
        {
            doc.Load(filePath);
            root = doc.SelectSingleNode("UserInfo") as XmlElement;
            fileExist = true;
        }
        else
        {
            root = doc.CreateElement("UserInfo");
        }

        XmlElement element = doc.CreateElement("Info");
        element.SetAttribute("UserName", userName.text);
        element.SetAttribute("Password", password.text);
        root.AppendChild(element);

        if (!fileExist)
        {
            doc.AppendChild(root);
        }

        doc.Save(filePath);
        accountDic.Add(userName.text, password.text);
        print("注册成功");
        alertDialog.SetActive(true);
        alertDialog.transform.Find("Text").GetComponent<Text>().text = "注册成功";
    }

    //点击注册按钮回调
    public void OnRegisterBtnClick()
    {
        print(userName.text);
        print(password.text);

        WriteAccountInfoFile();
    }

    //点击开始游戏按钮
    public void OnEnterGameBtnClick()
    {
        choosedName = GetComponentInChildren<CharacterChoosePanel>().name;
        //TODO : 点击进入游戏时给静态数据赋值
        //导入所有道具的信息
        ItemManager.Instance.Init(Application.streamingAssetsPath + "/AllItem.xml");

        //导入xml中的人物属性信息
        PlayerStaticInfo.Instance.Init(choosedName);
        //导入xml中的人物背包属性
        PlayerStaticInfo.Instance.InitBag(choosedName);

        GameManager.Instance.GivePlayerObj();
        SoundManager.Instance.Init();

        LevelManager.LoadLoadingLevel(PlayerStaticInfo.Instance.sceneName);
    }

    //获得账号信息在选择角色界面生成toggle预制体
    public void GetAccountCharacterInfo()
    {

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
            //如果账号符合
            if (account.Attributes["userName"].Value.Equals(GameManager.Instance.userName))
            {
                //加载toggle预制体
                GameObject toggle = Resources.Load<GameObject>("Prefabs/UI/CharacterInfo");
                //找寻生成的toggle挂载的父节点
                Transform parent = CharacterChooseBoard.transform.Find("Characters").transform;
                
                //根据所有的账号信息生成toggle组
                XmlNode character = account.SelectSingleNode("Character");
                while(character != null)
                {
                    GameObject obj = Instantiate(toggle);
                    obj.transform.SetParent(parent);
                    obj.GetComponent<Toggle>().group = parent.GetComponent<ToggleGroup>();
                    string name = character.Attributes["name"].Value;
                    obj.transform.Find("CharacterName").GetComponent<Text>().text = name;
                    string level = character.Attributes["level"].Value;
                    obj.transform.Find("CharacterLevel").GetComponent<Text>().text = "等级：" + level;
                    int type = int.Parse(character.Attributes["type"].Value);

                    switch(type)
                    {
                        case (int)PlayerType.HUMAN:
                            obj.transform.Find("CharacterType").GetComponent<Text>().text = "人族";
                            break;
                        case (int)PlayerType.WIZARDS:
                            obj.transform.Find("CharacterType").GetComponent<Text>().text = "仙族";
                            break;
                        case (int)PlayerType.DEMON:
                            obj.transform.Find("CharacterType").GetComponent<Text>().text = "妖族";
                            break;
                    }
                    
                    obj.transform.localScale = Vector3.one;
                    character = character.NextSibling;
                }
            }
            account = account.NextSibling;
        }

    }


    public void OnAlertConfirmBtnClick()
    {
        alertDialog.SetActive(false);
    }

}
