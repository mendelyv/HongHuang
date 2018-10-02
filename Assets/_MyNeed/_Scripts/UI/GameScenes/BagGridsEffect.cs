using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagGridsEffect : MonoBehaviour {

    private Image imageComponent;
    void Start()
    {
        imageComponent = GetComponent<Image>();
    }

    public void OnMouseEnter()
    {
        imageComponent.enabled = true;
    }

    public void OnMouseExit()
    {
        imageComponent.enabled = false;
    }

    

}
