using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCD : MonoBehaviour {

    public string name;
    public KeyCode skillKey;//技能的按键
    public float CDTime;//冷却时间
    public bool isCDing;//是否正在冷却
    public int skillCost;

    private Button skillBtn;
    private Image mask;//CD效果的遮罩
    private Text timeText;
    private float timer;
    private Transform skillPos;

    void Start()
    {
        //测试
        SkillEffectManager.Instance.Init();

        name = "canglongzhentian";
        isCDing = false;
        timer = 0.0f;
        skillBtn = GetComponent<Button>();
        mask = transform.Find("Mask").GetComponent<Image>();
        timeText = transform.Find("Text").GetComponent<Text>();
        skillPos = GameObject.FindGameObjectWithTag("Player").transform.Find("Bottom");
        mask.fillAmount = 0.0f;
    }

    void Update()
    {
        if(Input.GetKeyDown(skillKey))
        {
            UseSkill();
        }

        //如果正在CD
        if(isCDing)
        {
            timeText.text = timer.ToString();
            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                timer = 0.0f;
                isCDing = false;
                timeText.text = "";
            }
            mask.fillAmount = timer / CDTime;
        }


    }

    //使用技能
    public void UseSkill()
    {
        if(!isCDing && PlayerStaticInfo.Instance.currentMP >= skillCost)
        {
            Debug.Log("FangJiNeng");
            Instantiate(SkillEffectManager.Instance.skillDic[name],skillPos.position,Quaternion.identity);
            PlayerStaticInfo.Instance.currentMP -= skillCost;
            timer = CDTime;
            timeText.text = timer.ToString();
            isCDing = true;
        }
    }


    

}
