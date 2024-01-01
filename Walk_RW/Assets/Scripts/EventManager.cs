using System;
using UnityEngine;

public class EventManager : MonosingletonTemp<EventManager>
{
    public event Action<string> OnSceneLoaded;
    public event Action<string> OnGameEvent;

    // 触发场景加载完成事件
    public void TriggerSceneLoaded(string sceneName)
    {
        OnSceneLoaded?.Invoke(sceneName);
    }

    // 触发一般游戏事件
    public void TriggerGameEvent(string eventName)
    {
        OnGameEvent?.Invoke(eventName);
    }
}