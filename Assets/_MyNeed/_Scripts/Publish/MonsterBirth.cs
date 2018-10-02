using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBirth : MonoBehaviour
{
    public List<GameObject> Monster;//已生成怪物容器怪物
    public GameObject MonsterPre;//怪物预制体
    public int MonsterMaxNum;//怪物最大数量
    public float MonsterBirthdir = 5;//怪物生成距离间隔
    public float MonterBhTime = 5f;//怪物生成间隔
    //public float BirthTime=0.1f;
    //public float time=0f;
    void Start()
    {
        Monster = new List<GameObject>();
        for (int i = 0; i < MonsterMaxNum; i++)
        {
            //拷贝怪物预制体
            GameObject obj = Instantiate(MonsterPre,
                this.transform.position + new Vector3(i * MonsterBirthdir, 0, i * MonsterBirthdir), Quaternion.identity);
            //添加到容器中
            Monster.Add(obj);
            obj.transform.SetParent(this.transform);
        }
    }

    void Update()
    {
        if (Monster.Count < MonsterMaxNum)
        {
            Invoke("SpawnMonster", MonterBhTime);
        }
    }

    public void MonsterDeath()
    {
        if (Monster != null)
        {
            Monster.RemoveAt(0);
        }
    }//怪物死亡从容器删除

    public void SpawnMonster()//怪物数量减少重新刷新
    {
        for (int i = Monster.Count; i < MonsterMaxNum; i++)
        {
            GameObject obj = Instantiate(MonsterPre,
                this.transform.position + new Vector3(i * MonsterBirthdir, 0, i * MonsterBirthdir), Quaternion.identity);
            obj.transform.SetParent(this.transform);
            Monster.Add(obj);
        }
    }
}
