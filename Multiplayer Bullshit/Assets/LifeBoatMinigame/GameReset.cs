using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameReset : MonoBehaviour
{



    void OnTriggerEnter2D(Collider2D other){

        Destroy(other.gameObject);


        SceneManager.UnloadSceneAsync("LifeBoat Minigame");
        SceneManager.LoadScene(sceneName: "LifeBoat Minigame", LoadSceneMode.Additive);

    }






    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
