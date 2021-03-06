using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoInteract : SoundInteract
{
    public AudioClip song1;
    public AudioClip song2;
    public AudioClip song3;
    public AudioClip smash;
    public AudioClip c;
    public AudioClip d;
    public AudioClip e;
    public AudioClip f;
    public AudioClip g;
    public AudioClip a;
    public AudioClip b;
    public AudioClip c2;
    public AudioClip e2;
    public AudioClip g2;
    public AudioClip c3;
    public AudioClip[] clips;
    public List<AudioClip> loa;
    public void Start()
    {
       AudioClip[] clips = {song1 ,song2, song3, smash, c, d, e, f, g, a, b, c2, e2, g2, c3};
            loa = new List<AudioClip>(clips);
}
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
