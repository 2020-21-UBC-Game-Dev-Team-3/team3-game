using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInteract : Interactable
{
    public AudioSource audioSource;
    private void Awake()
    {
        audioSource.volume = PlayerPrefs.GetFloat("main volume");
    }

    public virtual void PlaySound()
    {
        audioSource.Play();
    }
}
