using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBoatGameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject LifeBoat;
    public GameObject Passenger1;
    

    private int score;
    public Text ScoreText;

    [SerializeField] int threshold;

    private bool isSafe;

    public bool isWin;

    



    void OnTrigger ( ){

    }

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
