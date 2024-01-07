using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    
    public string scenename = "Scene1";
    // Start is called before the first frame update
    // Update is called once per frame

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision");
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.LevelChange++;
            SceneManager.LoadScene(scenename);
            Debug.Log(GameManager.Instance.LevelChange);
        }
    }
}
