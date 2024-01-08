using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonosingletonTemp<GameManager>
{

    public int LevelChange = 0;
    public int body = 0;
    public GameOverScreen GameOverScreen;
    
    public void GameOver(int points)
    {
        body = points;
        GameOverScreen.Setup(body);
    }
    
    public void Init()
    {
        Debug.Log("GameManager Init");
    }
    
    // 每次进入新场景，都调用这个方法
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("GameManager OnSceneLoaded");

        // 寻找新的场景里面的带有GameOverScreen组件的东西，即便他没有被激活
        foreach (var gameOverScreen in Resources.FindObjectsOfTypeAll<GameOverScreen>())
        {
            if (gameOverScreen.gameObject.scene == scene)
            {
                GameOverScreen = gameOverScreen;
                break;
            }
        }

        //如果找到了,Debug
        if (GameOverScreen != null)
        {
            Debug.Log("GameOverScreen is not null");
        }
        else
        {
            Debug.Log("GameOverScreen is null");
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
