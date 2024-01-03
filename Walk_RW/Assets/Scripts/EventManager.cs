using System;
using UnityEngine;

public class EventManager : MonosingletonTemp<EventManager>
{
    public event Action<string> OnSceneLoaded;
    public event Action<GameEvent> OnGameEvent;
    
    public enum GameEvent
    {
        SceneLoaded,
        GameOver,
        // 添加更多的游戏事件...
    }

    // 触发场景加载完成事件
    /*
    public void TriggerSceneLoaded(string sceneName)
    {
        OnSceneLoaded?.Invoke(sceneName);
    }
    */

    // 触发一般游戏事件
    public void TriggerGameEvent(GameEvent eventName)
    {
        OnGameEvent?.Invoke(eventName);
    }
    
    

}