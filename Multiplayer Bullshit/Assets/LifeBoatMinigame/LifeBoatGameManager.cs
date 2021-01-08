using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LifeBoatGameManager : MonoBehaviour
{
    // Start is called before the first frame update

    //public GameObject LifeBoat;
    //public GameObject Passenger1;
    

    private int score = 0;
    public Text ScoreText;
    [SerializeField] int Passengers;

    public Text TaskComplete;

    [SerializeField] int threshold;

    private bool isSafe;

    public bool isWin;

    



   // void OnTriggerEnter2D (Collider2D other){ }

    void OnCollisionEnter2D (Collision2D other){
        Destroy(other.gameObject);
        score++;
       

    }

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ScoreText.text = "Passengers Saved:"+score+"/"+Passengers;


        if (score == Passengers){
            TaskComplete.gameObject.SetActive(true);
            SceneManager.LoadScene(sceneName: "Gaming", LoadSceneMode.Single);
        }


        
    }
}
