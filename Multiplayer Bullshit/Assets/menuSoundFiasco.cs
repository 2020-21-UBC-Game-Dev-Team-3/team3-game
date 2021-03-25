using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuSoundFiasco : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;
    void Start()
    {
        StartCoroutine(Unmute());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Unmute()
    {
        yield return new WaitForSeconds(1);
        audioSource.mute = false;
    }
}
