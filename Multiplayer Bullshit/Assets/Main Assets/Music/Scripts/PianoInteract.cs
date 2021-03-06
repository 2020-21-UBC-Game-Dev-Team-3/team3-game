using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoInteract : SoundInteract
{
    public List<AudioClip> loa;
    public override void PlaySound()
    {
        RandomClip();
        audioSource.Play();
    }
    public void RandomClip()
    {
        audioSource.clip = loa[Random.Range(0, loa.Count)];
    }
}


