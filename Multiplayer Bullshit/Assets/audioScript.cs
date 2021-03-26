using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioScript : MonoBehaviour
{
    public AudioSource audioSource;
    // Start is called before the first frame update
    private void Awake()
    {
        audioSource.volume = PlayerPrefs.GetFloat("main volume");
        audioSource.Play();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
