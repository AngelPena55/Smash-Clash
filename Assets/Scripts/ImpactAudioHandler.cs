using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactAudioHandler : MonoBehaviour
{
    public AudioSource audioSource;

    public void Start()
    {
        Destroy(gameObject, 10f);
    }

    public void Play()
    {
        transform.SetParent(null);
        StartCoroutine(PlayAudio());
    }

    IEnumerator PlayAudio()
    {
        audioSource.Play();
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        Destroy(gameObject);
    }
}
