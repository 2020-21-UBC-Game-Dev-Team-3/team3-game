using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrackScript : MonoBehaviour
{
    public bool dead;
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
