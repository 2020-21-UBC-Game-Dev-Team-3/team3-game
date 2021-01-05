using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using Cinemachine;
using System.Collections;





public class KillMechanic : MonoBehaviour
{

    Rigidbody myRB;
    Transform myAvatar;
    Animator myAnim;

    [SerializeField] public bool isImpostor;
    [SerializeField] InputAction KILL;

    //float killInput;

    public List<KillMechanic> targets;
    [SerializeField] Collider myCollider;

    public bool isDead;

    PhotonView PV; 

    [SerializeField] public GameObject BodyPrefab; 
    //[SerializeField] public GameObject GhostPrefab; 
    //[SerializeField] public GameObject BodyPrefab;  
    
    public GameObject Player;
    public Camera PerspectiveCamera;
    //public GameObject Ghost;



     // Start is called before the first frame update
    void Start()
    {

        PV = GetComponent<PhotonView>();
        if(!PV.IsMine)
        {
            return;
        }
        else{

        myAnim = GetComponent<Animator>();
        targets = new List<KillMechanic>();
        //targets = null;
        myRB = GetComponent<Rigidbody>();
        isDead = false;

        }
          
    }


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
       AddList(other);
    }

    private void OnTriggerExit (Collider other){
       RemoveList(other);
    }

    void AddList(Collider other){

        if(other.gameObject.tag == "Player"){
            KillMechanic tempTarget = other.gameObject.GetComponent<KillMechanic>();
           /* if(isImpostor){
                if(tempTarget.isImpostor)
                return;
                else
                {
                    */
                    targets.Add(tempTarget);
                    //Debug.Log(target.name);
                //}
            //}

        }


        
    }

    void RemoveList(Collider other){

         if (other.gameObject.tag == "Player"){
            KillMechanic tempTarget = other.GetComponent<KillMechanic>();
            if (targets.Contains(tempTarget)){
                targets.Remove(tempTarget);
            }
        }

        
    }

    void KillTarget (InputAction.CallbackContext context){

        if(!PV.IsMine)
        return;

      //  if(!isImpostor)
        //return;
         
        if (context.phase == InputActionPhase.Performed){
            if(targets.Count == 0 )
            return;
            else
            { 
                if(targets[targets.Count - 1].isDead){
                    return;
                }
                else{ 
               // transform.position = target.transform.position;
               // targets[targets.Count - 1].Die();

               targets[targets.Count - 1].PV.RPC("RPC_Kill",RpcTarget.All);
                
                targets.RemoveAt(targets.Count - 1);
                }
                

            }
        }
    }


    [PunRPC]
    void RPC_Kill() {
        Die();
    }

    public void Die(){

        if(!PV.IsMine)
        return;


        isDead = true;


        makeGhost();
        makeBody();


      


    }

     

    // Update is called once per frame
    void Update()
    {
       // if (!PV.IsMine)
       // return;
        
    }


    void makeGhost(){


        myAnim.SetBool("IsDead", isDead);
        Player.layer = LayerMask.NameToLayer("Ghost");
        Destroy(Player.GetComponent<Rigidbody>());
        myCollider.enabled = false;
        PerspectiveCamera.cullingMask |= 1 << LayerMask.NameToLayer("Ghost");
        Player.GetComponent<PlayerMovementController>().enabled = false;
        Player.GetComponent<GhostController>().enabled = true;

        
    }

    void makeBody(){

        
        Vector3 bodyPosition = new Vector3 (0,1f,0);
        //Quaternion bodyRotation = new Quaternion(90f,0,0,0);

        //body tempBody = Instantiate(BodyPrefab, transform.position, transform.rotation).GetComponent<body>();
        //body tempBody = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","body"), transform.position + bodyPosition, transform.rotation).GetComponent<body>(); 
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","body"), transform.position + bodyPosition, transform.rotation); 



    }
}


