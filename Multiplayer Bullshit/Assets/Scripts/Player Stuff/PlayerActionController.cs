﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PlayerActionController : MonoBehaviour, IDamageable
{
    [SerializeField] GameObject emergencyMeetingImage;
    [SerializeField] GameObject bodyReportedImage;
    [SerializeField] GameObject votingManager;

    MapManager mapMan;

    MinigameManager miniMan;

    public Interactable interactable = null;

    public PhotonView pv;

    PlayerManager playerMan;

    RoleAbility ability;

    private bool inVent = false;
    public Transform Vent1Pos;
    public Transform Vent2Pos;
    public Transform Vent3Pos;
    private float threshold = 1.0f; //magic number but it works and idk what else to do

    public GameObject minimap;
    public GameObject cam;
    public GameObject reticle;
    public GameObject sun;
    public string currMinigameSceneName;
    public string currMinigameObjectName;
    public bool minigameInterrupt;

    TaskBar tb;
    public bool tbIHolder = false;

    public DeathTrackScript dts;

    void Awake()
    {
        mapMan = GetComponent<MapManager>();
        miniMan = GetComponent<MinigameManager>();
        pv = GetComponent<PhotonView>();
        playerMan = PhotonView.Find((int)pv.InstantiationData[0]).GetComponent<PlayerManager>();
    }


    // Start is called before the first frame update
    void Start()
    {
        ability = GetComponent<TrapAbility>();

        Vent1Pos = GameObject.Find("Vent1Pos").transform;
        Vent2Pos = GameObject.Find("Vent2Pos").transform;
        Vent3Pos = GameObject.Find("Vent3Pos").transform;

        minimap = GameObject.Find("Minimap System");
        cam = GameObject.Find("Main Camera");
        reticle = GameObject.Find("Assassin Reticle");
        sun = GameObject.Find("sun");
        currMinigameSceneName = "none";
        minigameInterrupt = false;

        tb = GameObject.Find("Main Camera/TaskbarCanvas/Taskbar").GetComponent<TaskBar>();
        dts = GameObject.Find("DeathTrack").GetComponent<DeathTrackScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pv.IsMine) return;

        
        if (tbIHolder)
        {
            tb.IncrementTaskBar();
        }

        if (SceneManager.sceneCount > 1)
        {
            if (minigameInterrupt)
            {
                SceneManager.UnloadSceneAsync(currMinigameSceneName);
                exitMinigame(true);
                currMinigameSceneName = "none";
                currMinigameObjectName = "none";
                RenderSettings.ambientIntensity = 0.85f;
                RenderSettings.reflectionIntensity = 1f;
                RenderSettings.fogColor = new Color(177f, 161f, 185f, 255f);
            }
            else
            {
                return;
            }
            return;
        }
        else
        {
            if (currMinigameSceneName == "Rhythm Trap Minigame")
            {
                GetComponent<TrapAbility>().DecrementCurrTraps();
                interactable.gameObject.GetComponent<Trap>().Destroy();
            }
            else if (currMinigameSceneName != "none")
            {
                tbIHolder = true;
            }
            exitMinigame(true);
            miniMan.OnMinigameComplete(currMinigameObjectName);
            currMinigameSceneName = "none";
            currMinigameObjectName = "none";
            RenderSettings.ambientIntensity = 0.85f;
        }

        if (dts.dead)
        {
            TakeHit();
        }


        Interact();

        if (inVent)
        {
            CurrentlyInVent();
        }

        if (Input.GetKeyDown("q"))
        {
            ability.InitiateAbility();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable") || (other.CompareTag("MusicMaker")))
        {
            interactable = other.gameObject.GetComponent<Interactable>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable") || (other.CompareTag("MusicMaker")))
        {
            interactable = null;
        }
    }

    void Interact()
    {
        if (Input.GetKeyDown("e"))
        {
            if (!interactable.outline.enabled) return;
            ChooseInteractionEvent(interactable);
        }
    }

    void ChooseInteractionEvent(Interactable interactable)
    {
        switch (interactable.interactableName)
        {
            case "Emergency button":
                pv.RPC("TurnOnEmergencyPopUp", RpcTarget.All, "Emergency meeting");
                break;

            case "Dead body":
                pv.RPC("TurnOnEmergencyPopUp", RpcTarget.All, "Body reported");
                break;

            case "Vent":
                EnterVent(interactable);
                break;

            case "Scavenger hunt item":
                interactable.outline.enabled = false;
                interactable.gameObject.SetActive(false);
                FindObjectOfType<ScavengerHuntStarter>().CollectItem();
                break;

            case "Scavenger hunt minigame":
                if (miniMan.IsAssignedMinigame(interactable.interactableName))
                {
                    currMinigameObjectName = "Scavenger hunt minigame";
                    interactable.GetComponent<ScavengerHuntStarter>().ActivateScavengerHunt();
                }
                break;

            case "Interactable sound":
                interactable.GetComponent<SoundInteract>().PlaySound();
                break;

            case "Piano":
                pv.RPC("PianoInteract", RpcTarget.All, "Piano");
                break;

            case "Disarm trap":
                exitMinigame(false);
                currMinigameSceneName = "Rhythm Trap Minigame";
                SceneManager.LoadScene("Rhythm Trap Minigame", LoadSceneMode.Additive);
                break;

            case "Lifeboat minigame":
                if (miniMan.IsAssignedMinigame(interactable.interactableName))
                {
                    exitMinigame(false);
                    currMinigameSceneName = "LifeBoat Minigame";
                    currMinigameObjectName = "LifeBoat minigame";
                    SceneManager.LoadScene("LifeBoat Minigame", LoadSceneMode.Additive);
                }
                break;

            case "Lights minigame":
                exitMinigame(false);
                currMinigameSceneName = "Lights Minigame";
                RenderSettings.ambientIntensity = 0f;
                SceneManager.LoadScene("Lights Minigame", LoadSceneMode.Additive);
                break;

            case "Iceberg minigame":
                if (miniMan.IsAssignedMinigame(interactable.interactableName))
                {
                    exitMinigame(false);
                    currMinigameSceneName = "Icebergs";
                    currMinigameObjectName = "Iceberg minigame";
                    RenderSettings.reflectionIntensity = 0f;
                    RenderSettings.fogColor = new Color(0.7830189f, 0.7830189f, 0.7830189f, 0.7830189f);
                    SceneManager.LoadScene("Icebergs", LoadSceneMode.Additive);
                }
                break;

            case "Drink mixing minigame":
                if (miniMan.IsAssignedMinigame(interactable.interactableName))
                {
                    exitMinigame(false);
                    currMinigameSceneName = "DrinkMixingMinigame";
                    currMinigameObjectName = "Drink mixing minigame";
                    SceneManager.LoadScene("DrinkMixingMinigame", LoadSceneMode.Additive);
                }
                break;

            default:
                Debug.Log("not applicable");
                break;
        }
    }

    [PunRPC]
    public void PianoInteract(string name)
    {
        GameObject.Find(name).GetComponent<PianoInteract>().PlaySound();
    }

    [PunRPC]
    public void TurnOnEmergencyPopUp(string eventName)
    {
        if (eventName == "Emergency meeting")
        {
            StartCoroutine(ShowEmergencyPopUp(emergencyMeetingImage));
        }
        else if (eventName == "Body reported")
        {
            StartCoroutine(ShowEmergencyPopUp(bodyReportedImage));
        }
    }

    IEnumerator ShowEmergencyPopUp(GameObject eventImage)
    {
        eventImage.SetActive(true);
        PlayMakerFSM.BroadcastEvent("GlobalTurnMovementOff");
        TeleportPlayers();
        //TODO: ADD TELEPORTATION HERE
        yield return new WaitForSeconds(2);
        votingManager.SetActive(true);
        eventImage.SetActive(false);
    }

    void TeleportPlayers()
    {
        /*        int currTeleportLocation = 0;
                GameObject teleport = GameObject.Find("Teleport Locations");
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                foreach (GameObject player in players)
                {
                    player.transform.position = teleport.transform.GetChild(currTeleportLocation).transform.position;
                    currTeleportLocation++;
                }*/
    }

    public void TakeHit() => pv.RPC("RPC_TakeHit", RpcTarget.All);

    [PunRPC]
    public void RPC_TakeHit()
    {
        if (!pv.IsMine) return;
        mapMan.ResetMap();
        playerMan.Die();
    }

    void CurrentlyInVent()
    {
        if (Input.GetKeyDown("space"))
        {
            if ((transform.position - Vent1Pos.transform.position).sqrMagnitude < threshold)
            {
                transform.position = Vent2Pos.transform.position;
            }
            else if ((transform.position - Vent2Pos.transform.position).sqrMagnitude < threshold)
            {
                transform.position = Vent3Pos.transform.position;
            }
            else if ((transform.position - Vent3Pos.transform.position).sqrMagnitude < threshold)
            {
                transform.position = Vent1Pos.transform.position;
            }

        }
        else if (Input.GetKeyDown("a") || Input.GetKeyDown("w") || Input.GetKeyDown("d") || Input.GetKeyDown("s"))
        {
            pv.RPC("stopInvis", RpcTarget.All);
            inVent = false;
        }
    }

    void EnterVent(Interactable interactable)
    {
        inVent = true;
        transform.position = interactable.transform.GetChild(1).gameObject.transform.position;
        pv.RPC("turnInvis", RpcTarget.All);

    }

    [PunRPC]
    void turnInvis()//Interactable interactable)
    {

        //transform.position = interactable.transform.GetChild(1).gameObject.transform.position;
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(false);
    }
    [PunRPC]
    void stopInvis()//Interactable interactable)
    {

        //transform.position = interactable.transform.GetChild(1).gameObject.transform.position;
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.SetActive(true);
        transform.GetChild(4).gameObject.SetActive(true);
    }

    public void exitMinigame(bool exiting)
    {
        cam.SetActive(exiting);
        minimap.SetActive(exiting);
        reticle.SetActive(exiting);
        sun.SetActive(exiting);
        GetComponents<PlayMakerFSM>()[0].enabled = exiting;
        GetComponent<PlayMakerFixedUpdate>().enabled = exiting;
        GetComponent<PlayMakerLateUpdate>().enabled = exiting;
    }

}