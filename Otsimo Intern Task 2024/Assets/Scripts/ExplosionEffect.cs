using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    private AudioSource audioSource;

    public void ChangeColor(Color color)
    {
        GetComponent<ParticleSystem>().startColor = color;
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        Destroy(this.gameObject, 2f);
    }
}
