using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBodyManager : MonoBehaviour
{
    
    public ThrowHead throwHead;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //查找场景里面有ThrowHead的对象，并且指定给throwHead
        throwHead = GameObject.FindObjectOfType<ThrowHead>();

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("身体碰撞");
        // 检查碰撞的游戏对象是否是墙壁
        if (collider.gameObject.CompareTag("Wall"))
        {
            Debug.Log("身体碰撞墙壁");
            // 如果是墙壁，那么结束游戏
            GameOver();
        }
        if (collider.gameObject.CompareTag("Body"))
        {
            Debug.Log("身体碰撞身体");
            // 如果是身体，那么结束游戏
            GameOver();
        }
    }

    public void GameOver()
    {
        // 在这里添加结束游戏的代码
        Debug.Log("Game Over");
        
        //读取ThrowHead下的身体数量
        int bodycount = throwHead.bodyList.Count;
        GameManager.Instance.GameOver(bodycount);
        
        // 触发事件GameOver
        // 触发事件
        EventManager.Instance.TriggerGameEvent(EventManager.GameEvent.GameOver);
    }
}