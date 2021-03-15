using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);

    /*SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);*/
    SceneManager.UnloadSceneAsync("Rhythm Trap Minigame");
    SceneManager.LoadScene(sceneName: "Rhythm Trap Minigame", LoadSceneMode.Additive);
  }
}
