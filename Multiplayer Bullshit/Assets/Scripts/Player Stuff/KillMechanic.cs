using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class KillMechanic : MonoBehaviour
{

    Rigidbody myRB;
    Transform myAvatar;
    Animator myAnim;

    [SerializeField] bool isImpostor;
    [SerializeField] InputAction KILL;
    float killInput;

    List<KillMechanic> targets;
    [SerializeField] Collider myCollider;

    bool isDead;

    [SerializeField] GameObject bodyPrefab; 


    private void Awake() {
        KILL.performed += KillTarget;
    }


    private void OnEnable() {
        KILL.Enable();
    }

    private void OnDisable() {
        KILL.Disable();
    }

    void SetRole (bool newRole) {
        isImpostor = newRole;
    }

    private void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            KillMechanic tempTarget = other.GetComponent<KillMechanic>();
            if(isImpostor){
                if(tempTarget.isImpostor)
                return;
                else
                {
                    targets.Add(tempTarget);
                    //Debug.Log(target.name);
                }
            }

        }
    }

    private void OnTriggerExit (Collider other){
        if (other.tag == "Player"){
            KillMechanic tempTarget = other.GetComponent<KillMechanic>();
            if (targets.Contains(tempTarget)){
                targets.Remove(tempTarget);
            }
        }
    }

    void KillTarget (InputAction.CallbackContext context){
        if (context.phase == InputActionPhase.Performed){
            if(targets.Count == 0 )
            return;
            else
            {
                if(targets[targets.Count - 1].isDead){
                    return;
                }
               // transform.position = target.transform.position;
                targets[targets.Count - 1].Die();
                targets.RemoveAt(targets.Count - 1);
                

            }
        }
    }

    public void Die(){
        isDead = true;
        myAnim.SetBool("IsDead", isDead);
        myCollider.enabled = false;

      //  Body tempBody = Instantiate(body, transform.position, transform.rotation).GetComponent<body>();

    }

     

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        targets = new List<KillMechanic>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


