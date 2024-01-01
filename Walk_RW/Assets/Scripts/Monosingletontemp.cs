using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonosingletonTemp<T> : MonoBehaviour where T : MonosingletonTemp<T>
{
    private static T _instance;

    public static T Instance    
    {
        get
        {
            if (_instance == null)
            {
                new GameObject(typeof(T).Name).AddComponent<T>(); 
            }
            return _instance;
        }
    }

    void Awake()
    {

        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (_instance != this)
        {
            Destroy(this.gameObject);
        }

    }
}