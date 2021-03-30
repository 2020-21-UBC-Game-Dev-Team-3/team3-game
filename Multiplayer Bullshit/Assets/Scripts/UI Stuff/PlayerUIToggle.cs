using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIToggle : MonoBehaviour
{
    [SerializeField] List<GameObject> playerUI;

    public void TogglePlayerUI(bool toggle)
    {
        foreach(var ui in playerUI)
        {
            ui.SetActive(toggle);
        }
    }
}
