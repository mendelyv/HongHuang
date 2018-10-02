using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHeadFrame: MonoBehaviour {

    private Slider hpSlider;
    private Slider mpSlider;
    private Slider expSlider;

    private Text hpText;
    private Text mpText;
    private Text expText;

    void Start()
    {
        if (hpSlider == null)
            hpSlider = transform.Find("HP").GetComponent<Slider>();
        if (hpText == null)
            hpText = hpSlider.transform.GetComponentInChildren<Text>();

        if (mpSlider == null)
            mpSlider = transform.Find("MP").GetComponent<Slider>();
        if (mpText == null)
            mpText = mpSlider.transform.GetComponentInChildren<Text>();

        if (expSlider == null)
            expSlider = transform.Find("EXP").GetComponent<Slider>();
        if (expText == null)
            expText = expSlider.transform.GetComponentInChildren<Text>();
        
    }


    void Update()
    {
        float hpPrecent = (float)PlayerStaticInfo.Instance.currentHP / PlayerStaticInfo.Instance.maxHP;
        hpSlider.value = hpPrecent;
        hpText.text = PlayerStaticInfo.Instance.currentHP + " / " + PlayerStaticInfo.Instance.maxHP;

        float mpPrecent = (float)PlayerStaticInfo.Instance.currentMP / PlayerStaticInfo.Instance.maxMP;
        mpSlider.value = mpPrecent;
        mpText.text = PlayerStaticInfo.Instance.currentMP + " / " + PlayerStaticInfo.Instance.maxMP;

        float expPrecent = (float)PlayerStaticInfo.Instance.currentExp / PlayerStaticInfo.Instance.maxExp;
        expSlider.value = expPrecent;
        expText.text = PlayerStaticInfo.Instance.currentExp + " / " + PlayerStaticInfo.Instance.maxExp;
    }



}
