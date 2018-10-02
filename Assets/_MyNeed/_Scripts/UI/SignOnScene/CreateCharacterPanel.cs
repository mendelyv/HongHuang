using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCharacterPanel : MonoBehaviour {

    [Header("== 创建角色的模特 ==")]
    public GameObject humonModel;
    public GameObject wizardsModel;
    public GameObject demonModel;

    void Awake()
    {
        humonModel.SetActive(false);
        wizardsModel.SetActive(false);
        demonModel.SetActive(false);
    }

    public void OnHumonBtnClick()
    {
        humonModel.SetActive(true);
        wizardsModel.SetActive(false);
        demonModel.SetActive(false);
    }

    public void OnWizardBtnClick()
    {
        humonModel.SetActive(false);
        wizardsModel.SetActive(true);
        demonModel.SetActive(false);
    }

    public void OnDemonBtnClick()
    {
        humonModel.SetActive(false);
        wizardsModel.SetActive(false);
        demonModel.SetActive(true);
    }

    
}
