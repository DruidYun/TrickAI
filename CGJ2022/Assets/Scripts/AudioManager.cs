using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public enum AudioName
	{
        qiqiu = 1,
        yongchi = 2,
        feichuan = 3,
        puke = 4,
        fengshan = 5,
        zhongdian = 6,
        walk = 7,
        click = 8,
    }

    private static AudioManager _instance;
    public AudioClip[] bgmAudios;
    public AudioClip[] audios;

   // public AudioClip[] soundAudioClips;
    public static AudioManager Instace
    {
        get { return _instance; }

    }

    private void Awake()
    {
        if (_instance==null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        StartCoroutine(Audio());
    }
    // Update is called once per frame

    public void PlayAudio(AudioClip clip)
    {
        this.GetComponent<AudioSource>().PlayOneShot(clip,1f);
    }

    public void PlayAudio(AudioName clip)
    {
        this.GetComponent<AudioSource>().PlayOneShot(audios[(int)clip-1], 1f);
    }
    IEnumerator Audio()
    {  
        for (int i = 0; i < bgmAudios.Length; i++)
        {
            this.GetComponent<AudioSource>().clip = bgmAudios[i];
            this.GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(bgmAudios[i].length);
        }
    }
}
