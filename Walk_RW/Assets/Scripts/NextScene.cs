using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{

    public string scenename = "Scene1";
    public bool isLevel = true;
    private bool isReadyToChange = false;
    private bool wasReadyToChange = false;


    public Sprite sprite;

    public Sprite sprite2;
    // Start is called before the first frame update
    // Update is called once per frame

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision");
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.LevelChange++;
            SceneManager.LoadScene(scenename);
            Debug.Log(GameManager.Instance.LevelChange);
        }
    }

    void Update()
    {
        if(!wasReadyToChange && isReadyToChange)
        {
         //等一秒
            StartCoroutine(WaitAndPrint(0.25f));
        }

        wasReadyToChange = isReadyToChange;

        
        //如果是isLevel, 检测场上是否有Food
        if (isLevel)
        {
            //如果场上没有Food标签的东西
            if (GameObject.FindWithTag("Food") == null)
            {
                //如果场上没有Food,那么就加载下一个场景
                isReadyToChange = true;
            }

            //如果场上有Food
            else
            {
                //如果场上有Food,那么就不加载下一个场景
                isReadyToChange = false;
            }
        }
        else
        {
            isReadyToChange = true;
        }

        if (isReadyToChange == false)
        {
            //将贴图设置为贴图1
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
        }
        else
        {
            //将贴图设置为贴图2
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite2;
        }
        
        //携程
        IEnumerator WaitAndPrint(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            //等待之后执行的动作
            SoundManager.Instance.PlaySFX(SoundManager.Instance.AudioClipList[4]);

        }
    }
}
