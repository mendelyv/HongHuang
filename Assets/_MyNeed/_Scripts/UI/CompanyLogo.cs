using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanyLogo : MonoBehaviour {

    private string sceneName;
    void Start()
    {
        sceneName = "SignOn";
    }
    public void LoadSignOnScene()
    {
        LevelManager.LoadLoadingLevel(sceneName);
    }
}
