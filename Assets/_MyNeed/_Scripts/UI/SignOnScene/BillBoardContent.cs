using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BillBoardContent : MonoBehaviour {

    private Text content;

    public TextAsset textAsset;

    void Awake()
    {
        //textAsset = Resources.Load<TextAsset>("公告板.txt");
        content = GetComponent<Text>();

        content.text = textAsset.text;

    }
}
