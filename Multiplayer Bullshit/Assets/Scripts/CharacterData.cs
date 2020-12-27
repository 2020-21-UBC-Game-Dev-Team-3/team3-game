using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    [SerializeField] public string playerName;
    [SerializeField] public string playerColor;
    [SerializeField] public string playerRoom;

    public void getSet(string name, string color, string room)
    {
        playerName = name;
        playerColor = color;
        playerRoom = room;
    }
}
