using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour {

    private AudioSource audio;
    private Slider BGSlider;
    private Slider SESlider;
    
    void Start()
    {
        BGSlider = GameObject.FindGameObjectWithTag("UIRoot").transform.Find("SoundVolumePanel").Find("BGSlider").GetComponent<Slider>();
        audio = GetComponent<AudioSource>();
        //随机一个音乐
        audio.clip = SoundManager.Instance.clipList[Random.Range(0, SoundManager.Instance.clipList.Count)];
        audio.Play();
        audio.volume = SoundManager.Instance.BGvolume;
        BGSlider.value = audio.volume;

    }

    void Update()
    {
        audio.volume = BGSlider.value;
    }


}
