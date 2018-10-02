using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour {

    private GameObject Bag;
    private GameObject playerInfo;
    private GameObject soundVolumePanel;
    private GameObject ESCPanel;

    private Slider targetHeadFrame;//玩家鼠标点选目标的血量
    private Text targetHPText;//血量信息
    private Text targetName;//目标名字
    private PlayerAttack playerAttack;//玩家攻击脚本，用来获取mouseTarget

    void Start()
    {
        Bag = transform.Find("Bag").gameObject;
        playerInfo = transform.Find("PlayerInfo").gameObject;
        soundVolumePanel = transform.Find("SoundVolumePanel").gameObject;
        ESCPanel = transform.Find("ESCPanel").gameObject;

        targetHeadFrame = transform.Find("TargetHeadFrame").GetComponent<Slider>();
        targetHPText = targetHeadFrame.transform.Find("HPText").GetComponent<Text>();
        targetName = targetHeadFrame.transform.Find("Name").GetComponent<Text>();
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();

        //避免开始游戏时显示
        if (Bag.activeSelf)
            Bag.SetActive(false);
        if (playerInfo.activeSelf)
            playerInfo.SetActive(false);
        if (targetHeadFrame.gameObject.activeSelf)
            targetHeadFrame.gameObject.SetActive(false);
        if (soundVolumePanel.activeSelf)
            soundVolumePanel.SetActive(false);

    }

    void Update()
    {
        //按B显示背包界面和关闭背包界面
        if(Input.GetKeyDown(KeyCode.B))
        {
            Bag.SetActive(!Bag.activeSelf);
            
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            playerInfo.SetActive(!playerInfo.activeSelf);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ESCPanel.SetActive(!ESCPanel.activeSelf);
            if (soundVolumePanel.activeSelf)
                soundVolumePanel.SetActive(false);
        }

        //如果找到人物的脚本，并且脚本中的mouseTarget不为空
        //显示怪物的血条
        if(playerAttack != null && playerAttack.mouseTarget != null && playerAttack.mouseTarget.tag == "Enemy")
        {
            EnemyInfo tmp = playerAttack.mouseTarget.GetComponent<EnemyInfo>();
            targetHeadFrame.gameObject.SetActive(true);
            targetHeadFrame.value = (float)tmp.currentHP / tmp.maxHP;
            targetName.text = tmp.name;
            targetHPText.text = tmp.currentHP + " / " + tmp.maxHP;
        }
        else
        {
            targetHeadFrame.gameObject.SetActive(false);
        }

        
    }



}
