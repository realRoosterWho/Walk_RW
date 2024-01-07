using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
