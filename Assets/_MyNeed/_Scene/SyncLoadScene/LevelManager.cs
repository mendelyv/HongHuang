using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public static string nextLevelName;//用来告诉loading场景接下来要加载的场景名字
    public static Vector3 position;//切换场景人物出生的位置
    public static Vector3 rotition;//切换场景人物出生的角度

    private AsyncOperation async;

    public Slider slider;
    public Text text;
    public float tmpProgress;//使用一个差值使加载进度条不产生太大的跳动


    void Start()
    {
        tmpProgress = 0.0f;

        //如果目前活跃的界面为loading界面就加载下一界面
        if(SceneManager.GetActiveScene().name == "Loading")
        {
            async = SceneManager.LoadSceneAsync(nextLevelName);
            async.allowSceneActivation = false;
        }
    }
    
    public static void LoadLoadingLevel(string _nextLevelName)
    {
        nextLevelName = _nextLevelName;
        SceneManager.LoadScene("Loading");
    }

    void Update()
    {
        if(slider && text)
        {
            tmpProgress = Mathf.Lerp(tmpProgress, async.progress, Time.deltaTime * 5.0f);
            text.text = ((int)(tmpProgress / 9 * 10 * 100)).ToString() + " %";
            slider.value = tmpProgress / 9 * 10;

            if(tmpProgress / 9 * 10 > 0.99)
            {
                text.text = 100 + " %";
                slider.value = 1;
                async.allowSceneActivation = true;

                //场景加载完成后将UI和人物显示
                if (Transport.UIRoot != null)
                    Transport.UIRoot.SetActive(true);

                if (Transport.player != null)
                {
                    Transport.player.transform.position = position;
                    Transport.player.transform.eulerAngles = rotition;
                    Transport.player.SetActive(true);
                }

            }
        }
    }



}
