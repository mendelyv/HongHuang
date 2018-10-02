using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoFrame : MonoBehaviour {

    private Text HPText;
    private Text MPText;
    private Text AtkText;
    private Text DefText;

    private Transform propretyInfo;
    private Transform atkInfo;
    private Transform defInfo;
    private Transform equipGrids;

    void Start()
    {
        if (propretyInfo == null)
            propretyInfo = transform.Find("PropretyInfo").transform;
        if (atkInfo == null)
            atkInfo = transform.Find("AtkInfo").transform;
        if (defInfo == null)
            defInfo = transform.Find("DefInfo").transform;
        if (equipGrids == null)
            equipGrids = transform.Find("EquipGrids").transform;

        if(HPText == null)
            HPText = propretyInfo.Find("HPText").GetComponent<Text>();
        if (MPText == null)
            MPText = propretyInfo.Find("MPText").GetComponent<Text>();
        if (AtkText == null)
            AtkText = atkInfo.Find("AtkText").GetComponent<Text>();
        if (DefText == null)
            DefText = defInfo.Find("DefText").GetComponent<Text>();

    }

    void Update()
    {

        HPText.text = PlayerStaticInfo.Instance.currentHP.ToString();
        MPText.text = PlayerStaticInfo.Instance.currentMP.ToString();
        AtkText.text = PlayerStaticInfo.Instance.atk.ToString();
        DefText.text = PlayerStaticInfo.Instance.def.ToString();

    }


    public void OnXBtnClick()
    {
        transform.gameObject.SetActive(false);

    }


}
