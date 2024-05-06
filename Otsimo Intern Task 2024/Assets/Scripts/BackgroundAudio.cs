using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAudio : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip _backgroundMusic;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = _backgroundMusic;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(audioSource.time >= 31)
        {
            audioSource.Stop();
            audioSource.clip = null;
            audioSource.clip = _backgroundMusic;
            audioSource.Play();
        }
    }
}
