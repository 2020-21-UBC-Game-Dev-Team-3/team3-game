using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerCollision : MonoBehaviour
{
    public GameObject player;
    public Rigidbody player_rigid;
    private Vector3 startingPosition;
    
    void Start(){
        player_rigid = GetComponent<Rigidbody>();
        startingPosition = player.transform.position;
    }
    void OnCollisionEnter(Collision other){

        if(other.gameObject.tag == "Collider"){
            // player_rigid.constraints = RigidbodyConstraints.FreezePosition;
            player.transform.position = startingPosition;
            
            // SceneManager.LoadScene("Death");
        }

        if(other.gameObject.tag == "Wall"){
            player.transform.position = startingPosition;
        }
        
        
    }


    
}
