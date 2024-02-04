using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeLevel(int level)
    {
        //播放音效
        SoundManager.Instance.PlaySFX(SoundManager.Instance.AudioClipList[0]);
        
        //切换到场景"Level " + level
        SceneManager.LoadScene("Level " + level);

    }

    public void ChangeToSelect()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.AudioClipList[0]);

        //切换到场景"Select"
        SceneManager.LoadScene("Select");

    }

    public void ChangeToEndless()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.AudioClipList[0]);

        //切换到场景"Infinite"
        SceneManager.LoadScene("Endless");
    }

    public void ChangeToMain()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.AudioClipList[0]);
        SceneManager.LoadScene("Main");

        
    }
}
