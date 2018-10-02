using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnPlayer : MonoBehaviour {


    void Awake()
    {

        if(!GameManager.Instance.isPlayerAndUIBirth)
        {
            GameObject player = Instantiate(GameManager.Instance.playerObj,PlayerStaticInfo.Instance.scenePos,Quaternion.identity);

            player.GetComponent<NavMeshAgent>().enabled = false;
            player.GetComponent<NavMeshAgent>().enabled = true;

            //Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIRoot"));
            GameManager.Instance.isPlayerAndUIBirth = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

}
