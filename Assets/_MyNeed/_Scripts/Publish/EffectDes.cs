using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDes : MonoBehaviour {

    public float desTime = 2.0f;//自销毁时间

    void Start()
    {
        Destroy(this.gameObject, desTime);
    }

}
