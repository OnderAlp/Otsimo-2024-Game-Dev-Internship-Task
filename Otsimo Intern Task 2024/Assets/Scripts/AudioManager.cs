using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("Audio Manager is Null!");
            }

            return _instance;
        }
    }

    private AudioSource audioSource;

    public AudioClip _penSelected;
    public AudioClip _bucketSelected;
    public AudioClip _stampSelected;
    public AudioClip _eraserSelected;
    public AudioClip _colorSelected;
    public AudioClip _cannonSelected;
    public AudioClip _cannonFire;


    private void Awake()
    {
        _instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    public void SelectPen()
    {
        if(!audioSource.isPlaying)
        {
            audioSource.clip = _penSelected;
            audioSource.Play();
            StartCoroutine(AudioStoper(1));
        }
    }
    public void SelectBucket()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = _bucketSelected;
            audioSource.Play();

            StartCoroutine(AudioStoper(1));
        }
    }
    public void SelectStamp()
    {
        audioSource.clip = _stampSelected;
        audioSource.Play();
    }
    public void SelectEraser()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = _eraserSelected;
            audioSource.Play();

            StartCoroutine(AudioStoper(1));
        }
    }
    public void SelectColor()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = _colorSelected;
            audioSource.Play();
        }
    }
    public void SelectCannon()
    {
        audioSource.clip = _cannonSelected;
        audioSource.Play();

        StartCoroutine(AudioStoper(1));
    }
    public void FireCannon()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = _cannonFire;
            audioSource.Play();

            StartCoroutine(AudioStoper(1));
        }
    }

    private IEnumerator AudioStoper(int time)
    {
        yield return new WaitForSeconds(time);

        audioSource.Stop();
    }
}
