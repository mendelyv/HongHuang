using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transport : MonoBehaviour {

    public string sceneName;//要传送场景的名字
    public Vector3 position;
    public Vector3 rotition;
    
    public static GameObject UIRoot;//UI预制体
    public static GameObject player;//人物预制体
    


    void Start()
    {
        UIRoot = GameObject.FindGameObjectWithTag("UIRoot");
        player = GameObject.FindGameObjectWithTag("Player");
        DontDestroyOnLoad(UIRoot);
        DontDestroyOnLoad(player);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            //传送时将不需要销毁的隐藏
            UIRoot.SetActive(false);
            //设置人物不能移动，避免在切换场景时还在做射线碰撞检测
            player.GetComponent<PlayerController>().isMoving = false;
            player.SetActive(false);

            //给场景传送完成后人物要出现的位置赋值
            LevelManager.position = position;
            LevelManager.rotition = rotition;
            LevelManager.LoadLoadingLevel(sceneName);
        }
    }

    
}
