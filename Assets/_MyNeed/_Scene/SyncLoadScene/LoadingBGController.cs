using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBGController : MonoBehaviour {

    public Sprite[] BGSprite;

    private Image imageComponent;

    void Start()
    {
        imageComponent = GetComponent<Image>();

        imageComponent.sprite = BGSprite[(int)Random.Range(0, 5)];
    }

}
