using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DartsPointSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public enum GameState 
    {
        T1,
        T2,
        T3,
        T4,
        T5,
        WIN
    }
    private GameState currentState;
    void Start()
    {
        currentState = GameState.T1;
    }

    // Update is called once per frame
    public void ClickOnTarget(int target)
    {
        switch (currentState)
        {
            case GameState.T1:
                if (target == 1) { currentState = GameState.T2; Destroy(GameObject.Find("Target1")); };
                break;
            case GameState.T2:
                if (target == 2) { currentState = GameState.T3; Destroy(GameObject.Find("Target2")); };
                break;
            case GameState.T3:
                if (target == 3) { currentState = GameState.T4; Destroy(GameObject.Find("Target3")); };
                break;
            case GameState.T4:
                if (target == 4) { currentState = GameState.T5; Destroy(GameObject.Find("Target4")); };
                break;
            case GameState.T5:
                if (target == 5) { currentState = GameState.WIN; Destroy(GameObject.Find("Target5"));
                    StartCoroutine(EndGame(2));
                }; // also should put a win text or smt
                break;
            default:
                Debug.Log("Victory");
                // Have some fn that checks off win and loads back to gaming scene
                break;
        }
    }
    IEnumerator EndGame(int time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.UnloadSceneAsync("Darts Minigame");
    }
}
