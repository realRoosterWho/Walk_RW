using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBodyManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 检查碰撞的游戏对象是否是墙壁
        if (collision.gameObject.CompareTag("Wall"))
        {
            // 如果是墙壁，那么结束游戏
            GameOver();
        }
    }

    public void GameOver()
    {
        // 在这里添加结束游戏的代码
        Debug.Log("Game Over");
        
        // 触发事件GameOver
        // 触发事件
        EventManager.Instance.TriggerGameEvent(EventManager.GameEvent.GameOver);
        
    }
}