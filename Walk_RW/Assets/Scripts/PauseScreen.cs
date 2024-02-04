using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    // Start is called before the first frame update

    public void Setup()
    {
        gameObject.SetActive(true); //激活自己
    }
    
    public void SetDown()
    {
        gameObject.SetActive(false); //关闭自己
    }
    
    public void QuitButton()
    {
        //退出游戏
        Application.Quit();
    }


    public void RestartButton()
    {
        //加载当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
        
    public void ResumeButton()
    {
        //继续游戏
        GameManager.Instance.ResumeGame();
    }
        
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
