using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCPanel : MonoBehaviour {

    private GameObject soundVolumePanel;

    void Start()
    {
        soundVolumePanel = transform.parent.transform.Find("SoundVolumePanel").gameObject;

        if (soundVolumePanel.activeSelf)
            soundVolumePanel.SetActive(false);
    }

    public void OnSoundVolumeBtnClick()
    {
        soundVolumePanel.SetActive(true);
    }

    public void OnXBtnClick()
    {
        transform.gameObject.SetActive(false);
    }

    public void OnExitGameBtnClick()
    {
        Application.Quit();
    }

}
