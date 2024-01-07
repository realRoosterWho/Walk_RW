using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI pointsText;

    public void Setup(int points)
    {
        gameObject.SetActive(true);
        pointsText.text = points.ToString() + " points";
        Debug.Log("Points" + pointsText.text);
    }


    public void RestartButton()
    {
        //加载当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
        
        
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
