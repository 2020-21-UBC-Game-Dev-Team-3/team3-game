using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInteract : Interactable
{
    public AudioSource audioSource;

    public virtual void PlaySound()
    {
        audioSource.Play();
    }
}
