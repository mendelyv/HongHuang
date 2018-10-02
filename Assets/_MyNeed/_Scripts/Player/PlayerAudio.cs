using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAudio : MonoBehaviour {

    private AudioSource audio;

    private Dictionary<string, AudioClip> clipDic;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        clipDic = new Dictionary<string, AudioClip>();

        clipDic.Add("攻击", Resources.Load<AudioClip>("Audio/Skill/Wound/BeiJi_SheJi_2"));
        audio.clip = clipDic["攻击"];
    }
}
