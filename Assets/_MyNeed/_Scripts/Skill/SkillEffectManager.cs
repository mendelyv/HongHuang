using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectManager  {

    private static SkillEffectManager instance;
    public static SkillEffectManager Instance
    {
        get
        {
            if (instance == null)
                instance = new SkillEffectManager();
            return instance;
        }
    }


    public Dictionary<string, GameObject> skillDic;

    public void Init()
    {
        skillDic = new Dictionary<string, GameObject>();
        GameObject obj = Resources.Load<GameObject>("Prefabs/Effect/canglongzhentian");
        skillDic.Add("canglongzhentian", obj);
    }
}
