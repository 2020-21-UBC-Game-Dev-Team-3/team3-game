using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using Cinemachine;



public class KillMechanic : MonoBehaviour
{

    Rigidbody myRB;
    Transform myAvatar;
    Animator myAnim;

    [SerializeField] public bool isImpostor;
    [SerializeField] InputAction KILL;

    //float killInput;
    public GameObject deadPlayer;
    public GameObject bodyPlayer;
    public PhotonView photon;
    public List<KillMechanic> targets;
    [SerializeField] Collider myCollider;
    public GameObject[] playerList;
    public GameObject[] bodyList;
    public bool isDead;
    public PhotonView targetPhoton;
    public PhotonView PV;
    public List<Component> components;
    [SerializeField] public GameObject BodyPrefab;
    //[SerializeField] public GameObject GhostPrefab; 
    //[SerializeField] public GameObject BodyPrefab;  
    public string targetName;
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
                    targetPhoton = targets[targets.Count - 1].GetComponent<PhotonView>();
                    targetName = (string)targetPhoton.Owner.NickName;
                   // Debug.Log(targetName);
                    PV.RPC("RPC_Kill", RpcTarget.All, targetName);
                    //PV.RPC("makeBody", RpcTarget.MasterClient);

                
                targets.RemoveAt(targets.Count - 1);
                }
                

            }
        }
    }


    [PunRPC]
    public void RPC_Kill(string targetName) {
        Die(targetName);
    }

    public void Die(string targetName){

        //PhotonNetwork.PlayerList;
        foreach (Player player in PhotonNetwork.PlayerList) {
            if (player.NickName == targetName) {
                makeGhost(player);
                makeBody(player);
            }

        }

      


    }

     

    // Update is called once per frame
    void Update()
    {
       // if (!PV.IsMine)
       // return;
        
    }


    void makeGhost(Player player){

        playerList = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject gameObj in playerList)
        {
            photon = gameObj.GetComponent<PhotonView>();
            if (photon.Controller.NickName == player.NickName)
            {
                deadPlayer = gameObj;
                /*        myAnim.SetBool("IsDead", isDead);*/
        deadPlayer.layer = LayerMask.NameToLayer("Ghost");
        Destroy(deadPlayer.GetComponent<Rigidbody>());
                for (int i = 0; i < 3; i++)
                {
                    //Component component = deadPlayer.transform.GetChild(i);
                    deadPlayer.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Ghost");
                /*for(int j = 0; j < component.transform.childCount; j++)    
                {
                    component.transform.GetChild(j).gameObject.layer = LayerMask.NameToLayer("Ghost");
                }*/
                }

               // for(int i = 0; i <; i++)
        //myCollider.enabled = false;
        deadPlayer.tag = "Ghost";
     /*        PerspectiveCamera.cullingMask |= 1 << LayerMask.NameToLayer("Ghost");*/
        deadPlayer.GetComponent<PlayerMovementController>().enabled = false;
        deadPlayer.GetComponent<GhostController>().enabled = true;
                deadPlayer = null;
            }
            photon = null;
        }



        
    }
   // [PunRPC]
    void makeBody(Player player){

          bodyList = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject gameObj in playerList)
        {
            if (photon.Controller.NickName == player.NickName)
            {
                bodyPlayer = gameObj;

                 Vector3 bodyPosition = new Vector3 (0,1f,0);
        //Quaternion bodyRotation = new Quaternion(90f,0,0,0);

        //body tempBody = Instantiate(BodyPrefab, transform.position, transform.rotation).GetComponent<body>();
        //body tempBody = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","body"), transform.position + bodyPosition, transform.rotation).GetComponent<body>(); 
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs","body"), bodyPlayer.transform.position + bodyPosition, bodyPlayer.transform.rotation); 
               
                }

                bodyPlayer = null;
            }
        }

        
       



}