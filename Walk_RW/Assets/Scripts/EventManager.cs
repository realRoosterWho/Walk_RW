using System;
using UnityEngine;
using System.Collections.Generic; // Add this line


public class GameEventArgs : EventArgs
{
    public int IntValue { get; set; }
    public float FloatValue { get; set; }
    public string StringValue { get; set; }
    public List<Vector3> Vector3ListValue { get; set; }
    
}

public class EventManager : MonosingletonTemp<EventManager>
{
    public event Action<string> OnSceneLoaded;
    public event Action<GameEvent, GameEventArgs> OnGameEvent; // 修改事件的类型    
    public enum GameEvent
    {
        SceneLoaded,
        GameOver,
        OnMove,
        OneMove,
        // 添加更多的游戏事件...
    }
    

    // 触发一般游戏事件
    public void TriggerGameEvent(GameEvent eventName, GameEventArgs eventArgs = null) // 添加一个新的参数
    {
        OnGameEvent?.Invoke(eventName, eventArgs);
    }
    
    

    // 触发示例
    /*
    EventManager.Instance.TriggerGameEvent(EventManager.GameEvent.OneMove, 
    new GameEventArgs { IntValue = 1, FloatValue = 2.0f, StringValue = "test", PlayerInfo = playerInfo }); 
    */
    // 监听示例：
    /*
    void Start()
    {
        EventManager.Instance.OnGameEvent += OnMove; //订阅事件
    }
     */
    // 触发示例
    /*
    void OnMove(EventManager.GameEvent gameEvent) //事件处理函数
       { 
       Debug.Log("AIOnMove");
       }
     */
}