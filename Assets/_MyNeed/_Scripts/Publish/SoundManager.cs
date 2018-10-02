using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager  {

    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
                instance = new SoundManager();
            return instance;
        }
    }

    

    public List<AudioClip> clipList;

    public float BGvolume;
    public float SEvolume;

    private bool isInit = false;

    public void Init()
    {
        if (isInit)
            return;

        clipList = new List<AudioClip>();

        for(int i = 1; i <= 5; i++)
        {
            clipList.Add(Resources.Load<AudioClip>("Audio/BG" + i));
        }

        BGvolume = 0.5f;
        SEvolume = 0.5f;
        isInit = true;

    }

}
