using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public GameObject enemyPre;
    public int enemyNum;
    public Transform[] enemyPos;
    public float spawnEnemyBetween;//刷怪的间隔时间

    private List<GameObject> enemyList;
    private float timer;

    void Start()
    {
        enemyList = new List<GameObject>();
        
        for(int i = 0; i < enemyNum; i++)
        {
            GameObject obj = Instantiate(enemyPre,enemyPos[i].position,Quaternion.identity);
            //obj.transform.SetParent(this.transform);
            //obj.transform.position = ;
        }
    }

    void Update()
    {
        //如果怪物的数量少于地图所需要的数量
        if(enemyList.Count < enemyNum)
        {
            timer += Time.deltaTime;
            if(timer > spawnEnemyBetween)
            {
                timer = 0.0f;
                //GameObject obj = Instantiate(enemyPre);
                Debug.Log("SpawnEnemy");
                //for(int i = 0; i < enemyPos.Length; i++)
                //{
                //    if(enemyPos[i])
                //}
            }
        }

    }
}
