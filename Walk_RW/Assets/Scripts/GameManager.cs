using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonosingletonTemp<GameManager>
{

    public int LevelChange = 0;
    public int body = 0;
    public GameOverScreen GameOverScreen;
    public PauseScreen PauseScreen;
    public bool isPaused = false;
    
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


		//如果当前场景名称是Congra，那么播放音乐
		if (SceneManager.GetActiveScene().name == "Congra")
	    {
		    SoundManager.Instance.PlayMusic(SoundManager.Instance.MusicClipList[1]);
		}
		//如果当前场景名称是Main，那么播放音乐
		else if (SceneManager.GetActiveScene().name == "Main" | SceneManager.GetActiveScene().name == "Select")
	    {
		    SoundManager.Instance.PlayMusic(SoundManager.Instance.MusicClipList[1]);
		}
		else
		{
			SoundManager.Instance.PlayMusic(SoundManager.Instance.MusicClipList[0]);
		}
		

        // 寻找新的场景里面的带有GameOverScreen组件的东西，即便他没有被激活
        foreach (var gameOverScreen in Resources.FindObjectsOfTypeAll<GameOverScreen>())
        {
            if (gameOverScreen.gameObject.scene == scene)
            {
                GameOverScreen = gameOverScreen;
                break;
            }
        }
        
        // 寻找新的场景里面的带有PauseScreen组件的东西，即便他没有被激活
        foreach (var pauseScreen in Resources.FindObjectsOfTypeAll<PauseScreen>())
        {
            if (pauseScreen.gameObject.scene == scene)
            {
                PauseScreen = pauseScreen;
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
    
    public void PauseGame()
    {
        //暂停游戏
        Time.timeScale = 0;
        PauseScreen.Setup();
        //设置isPaused为true
        isPaused = true;
    }
    
    public void ResumeGame()
    {
        //继续游戏
        Time.timeScale = 1;
        PauseScreen.SetDown();
        //设置isPaused为false
        isPaused = false;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        //如果当前场景名称是Congra，那么播放音乐
        if (SceneManager.GetActiveScene().name == "Congra")
        {
            SoundManager.Instance.PlayMusic(SoundManager.Instance.MusicClipList[1]);
        }
        //如果当前场景名称是Main，那么播放音乐
        else if (SceneManager.GetActiveScene().name == "Main" | SceneManager.GetActiveScene().name == "Select")
        {
            SoundManager.Instance.PlayMusic(SoundManager.Instance.MusicClipList[1]);
        }
        else
        {
            SoundManager.Instance.PlayMusic(SoundManager.Instance.MusicClipList[0]);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //如果按下了ESC键
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //如果当前游戏是暂停状态
            if (GameManager.Instance.isPaused == true)
            {
                //继续游戏
                GameManager.Instance.ResumeGame();
            }
            else
            {
                //暂停游戏
                GameManager.Instance.PauseGame();
            }
        }
    }
}
