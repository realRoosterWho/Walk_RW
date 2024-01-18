using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Video;

public class SoundManager : MonosingletonTemp<SoundManager>
{
    // Start is called before the first frame update
    public AudioSource musicSource;
    public AudioSource sfxSource;
    
    //初始化一个AudioClipList
    public List<AudioClip> AudioClipList = new List<AudioClip>();
    
    public List<AudioClip> MusicClipList = new List<AudioClip>();
    
    public void Init()
    {
        Debug.Log("SoundManager Init");
    }
    public void PlayMusic(AudioClip clip, float volume = 0.3f, bool loop = true)
    {
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.volume = volume;
        musicSource.Play();
    }
    
    public void PlaySFX(AudioClip clip, float volume = 0.3f)
    {
        sfxSource.clip = clip;
        sfxSource.volume = volume;
        sfxSource.PlayOneShot(clip);
    }
    
    public void EnableSFX(AudioClip clip, AudioMixerGroup mixer = null)
    {
        sfxSource.clip = clip;
        sfxSource.outputAudioMixerGroup = mixer;
        // 如果在播放，什么都不做，否则播放
        if (sfxSource.isPlaying)
        {
            return;
        }
        sfxSource.Play();
        sfxSource.enabled = true;
    }
    
    public void DisableSFX()
    {
        sfxSource.enabled = false;
    }

    private void Start()
    {
        
        //给自己添加一个AudioSource组件
        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
        
        //播放背景音乐
        PlayMusic(MusicClipList[0]);
    }

    private void Update()
    {

    }
}
