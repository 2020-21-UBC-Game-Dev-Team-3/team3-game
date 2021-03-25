using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PlayerActionController : MonoBehaviour, IDamageable {
  [SerializeField] GameObject emergencyMeetingImage;
  [SerializeField] GameObject bodyReportedImage;
  [SerializeField] GameObject votingManager;
  [SerializeField] List<AudioSource> reportedAudios;
  [SerializeField] List<AudioSource> meetingAudios;
  [SerializeField] List<AudioSource> bgmAudios;
  [SerializeField] GameObject[] players;
  MapManager mapMan;

  MinigameManager miniMan;

  PlayerManager playerMan;

  public Interactable interactable = null;

  public PhotonView pv;

  RoleAbility ability;

  bool subRoleAssigned;
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
  public string currMinigameObjectName = "none";
  public bool minigameInterrupt;

  TaskBar tb;
  public bool tbIHolder = false;

  public DeathTrackScript dts;

  void Awake() {
    mapMan = GetComponent<MapManager>();
    miniMan = GetComponent<MinigameManager>();
    pv = GetComponent<PhotonView>();
    playerMan = PhotonView.Find((int)pv.InstantiationData[0]).GetComponent<PlayerManager>();

        //if (gameObject.CompareTag("Ghost")) OnStartGame();
  }


  // Start is called before the first frame update
  void Start() {
    Vent1Pos = GameObject.Find("Vent1Pos").transform;
    Vent2Pos = GameObject.Find("Vent2Pos").transform;
    Vent3Pos = GameObject.Find("Vent3Pos").transform;

    foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[]) {
      if (go.name == "Minimap System") {
        minimap = go;
        minimap.SetActive(false);
      }
      if (go.name == "Assassin Reticle") {
        reticle = go;
        reticle.SetActive(false);
      }
    }
    //minimap = null;

    dts = null;
    cam = GameObject.Find("Main Camera");
    //reticle = GameObject.Find("Assassin Reticle");
    sun = GameObject.Find("sun");
    currMinigameSceneName = "none";
    minigameInterrupt = false;

  }

  public void OnStartGame() {
    //minimap = GameObject.Find("Minimap System");
    //minimap.SetActive(true);
        Debug.Log("going to get taskbar");
    tb = GameObject.Find("Main Camera/Taskbar Canvas/Taskbar").GetComponent<TaskBar>();
        Debug.Log("is task bar there?: " + tb != null);
    dts = GameObject.Find("DeathTrack").GetComponent<DeathTrackScript>();
  }

  public void InitiateRoleAbilityAssignment() {
    pv.RPC("SetUpRoleAbility", RpcTarget.All);
  }

  [PunRPC]
  void SetUpRoleAbility() {
    if (!pv.IsMine && subRoleAssigned) return;
    Role playerRole = GetComponent<Role>();

    switch (playerRole.subRole) {
      case Role.Roles.Assassin:
        ability = GetComponent<AssassinAbility>();
        ability.enabled = true;
        ability.SetAbilityText();
        reticle.SetActive(true);
        playerMan.assignedRole = Role.Roles.Imposter;
        break;

      case Role.Roles.Chameleon:
        ability = GetComponent<ChameleonAbility>();
        reticle.SetActive(true);
        ability.enabled = true;
        ability.SetAbilityText();
        playerMan.assignedRole = Role.Roles.Imposter;
        break;

      case Role.Roles.Trapper:
        ability = GetComponent<TrapAbility>();
        ability.enabled = true;
        ability.SetAbilityText();
        playerMan.assignedRole = Role.Roles.Imposter;
        break;

      case Role.Roles.Disarmer:
        ability = GetComponent<DisarmerAbility>();
        ability.enabled = true;
        ability.SetAbilityText();
        playerMan.assignedRole = Role.Roles.Crewmate;
        break;

      case Role.Roles.None:
        ability = null;
        playerMan.assignedRole = Role.Roles.Crewmate;
        break;

      default:
        Debug.Log("no ability to give");
        break;
    }

    subRoleAssigned = true;
  }

  // Update is called once per frame
  void Update() {
    if (!pv.IsMine) return;


    if (tbIHolder) {
      //@Adrienne had to move this from start to update
      //tb = GameObject.Find("Main Camera/TaskbarCanvas/Taskbar").GetComponent<TaskBar>();
      tb.IncrementTaskBar();
    }

    if (SceneManager.sceneCount > 1) {
      if (minigameInterrupt) {
        SceneManager.UnloadSceneAsync(currMinigameSceneName);
        exitMinigame(true);
        //miniMan.OnMinigameComplete(currMinigameObjectName);
        currMinigameSceneName = "none";
        currMinigameObjectName = "none";
        RenderSettings.ambientIntensity = 0.85f;
      } else {
        return;
      }
      return;
    } else {
      minigameInterrupt = false;
      if (currMinigameSceneName == "Rhythm Trap Minigame" || currMinigameSceneName == "Chance Trap Minigame") {
/*        interactable.gameObject.GetComponent<Trap>().Destroy();*/
      } else if (currMinigameSceneName != "none") {
        tbIHolder = true;
      }
      //@Adrienne this is the line I had to comment out in order for players to select character skin
      exitMinigame(true);
      miniMan.OnMinigameComplete(currMinigameObjectName);
      currMinigameSceneName = "none";
      currMinigameObjectName = "none";
      RenderSettings.ambientIntensity = 0.85f;
    }

    //@Adrienne had to move this from start to update
    dts = GameObject.Find("DeathTrack").GetComponent<DeathTrackScript>();
    if (dts != null && dts.dead) {
      TakeHit();
    }


    Interact();

    if (inVent) {
      CurrentlyInVent();
    }

    if (Input.GetKeyDown("q")) {
      ability.InitiateAbility();
    }

    if (Input.GetKeyDown(KeyCode.V)) {
      FindObjectOfType<TaskBar>().IncrementTaskBar();
      FindObjectOfType<TaskBar>().IncrementTaskBar();
    }
  }

  void OnTriggerEnter(Collider other) {
    if (other.CompareTag("Interactable") || (other.CompareTag("MusicMaker"))) {
      interactable = other.gameObject.GetComponent<Interactable>();
    }
  }

  void OnTriggerExit(Collider other) {
    if (other.CompareTag("Interactable") || (other.CompareTag("MusicMaker"))) {
      interactable = null;
    }
  }

  void Interact() {
    if (Input.GetKeyDown("e")) {
      if (!interactable.outline.enabled) return;
      ChooseInteractionEvent(interactable);
    }
  }
    [PunRPC]
    void VentSound(string ventName)
    {
        GameObject.Find(ventName).GetComponent<AudioSource>().Play();
    }

  void ChooseInteractionEvent(Interactable interactable) {
    switch (interactable.interactableName) {
      case "Character select":
        FindObjectOfType<GameManager>().OpenCharacterSelectCanvas();
        break;

      case "Emergency button":
        pv.RPC("TurnOnEmergencyPopUp", RpcTarget.All, "Emergency meeting");

        break;

      case "Dead body":
        pv.RPC("TurnOnEmergencyPopUp", RpcTarget.All, "Body reported");
        break;

      case "Vent":
        if (GetComponent<Role>().currRole == Role.Roles.Imposter) {
                    pv.RPC("VentSound", RpcTarget.All, interactable.name);
          EnterVent(interactable);
        } else {
          Debug.Log("You're not an imposter!");
        }
        break;

      case "Scavenger hunt item":
        interactable.outline.enabled = false;
        interactable.gameObject.SetActive(false);
        FindObjectOfType<ScavengerHuntStarter>().CollectItem();
        break;

      case "Scavenger hunt minigame":
        if (miniMan.IsAssignedMinigame(interactable.interactableName)) {
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
        interactable.GetComponent<Trap>().Destroy();
        exitMinigame(false);
        currMinigameSceneName = "Rhythm Trap Minigame";
        SceneManager.LoadScene("Rhythm Trap Minigame", LoadSceneMode.Additive);
        break;

      case "Lifeboat minigame":
        if (miniMan.IsAssignedMinigame(interactable.interactableName)) {
          exitMinigame(false);
          currMinigameSceneName = "LifeBoat Minigame";
          currMinigameObjectName = "Lifeboat minigame";
          SceneManager.LoadScene("LifeBoat Minigame", LoadSceneMode.Additive);
        }
        break;

      case "Darts minigame":
        if (miniMan.IsAssignedMinigame(interactable.interactableName)) {
          exitMinigame(false);
          currMinigameSceneName = "Darts Minigame";
          currMinigameObjectName = "Darts minigame";
          SceneManager.LoadScene("Darts Minigame", LoadSceneMode.Additive);
        }
        break;

      case "Lights minigame":
        exitMinigame(false);
        currMinigameSceneName = "Lights Minigame";
        RenderSettings.ambientIntensity = 0f;
        SceneManager.LoadScene("Lights Minigame", LoadSceneMode.Additive);
        break;

      case "Iceberg minigame":
        if (miniMan.IsAssignedMinigame(interactable.interactableName)) {
          exitMinigame(false);
          currMinigameSceneName = "Icebergs";
          currMinigameObjectName = "Iceberg minigame";
          RenderSettings.reflectionIntensity = 0f;
          RenderSettings.fogColor = new Color(0.7830189f, 0.7830189f, 0.7830189f, 0.7830189f);
          SceneManager.LoadScene("Icebergs", LoadSceneMode.Additive);
        }
        break;

      case "Drink mixing minigame":
        if (miniMan.IsAssignedMinigame(interactable.interactableName)) {
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
  public void PianoInteract(string name) {
    GameObject.Find(name).GetComponent<PianoInteract>().PlaySound();
  }

  [PunRPC]
  public void TurnOnEmergencyPopUp(string eventName) {
    if (eventName == "Emergency meeting") {
      StartCoroutine(ShowEmergencyPopUp(emergencyMeetingImage));
    } else if (eventName == "Body reported") {
      StartCoroutine(ShowEmergencyPopUp(bodyReportedImage));
    }
  }

  IEnumerator ShowEmergencyPopUp(GameObject eventImage) {
    pv.RPC("MeetingAudioRPC", RpcTarget.All);
    minigameInterrupt = true;
    eventImage.SetActive(true);
    PlayMakerFSM.BroadcastEvent("GlobalTurnMovementOff");
    FindObjectOfType<GameManager>().TeleportPlayers();
    yield return new WaitForSeconds(2);
    votingManager.SetActive(true);
    eventImage.SetActive(false);
  }
  [PunRPC]
  public void MeetingAudioRPC() {
    players = GameObject.FindGameObjectsWithTag("Player");
    foreach (GameObject p in players) {
      reportedAudios.Add(p.transform.Find("ReportAudio").GetComponent<AudioSource>());
    }
    foreach (AudioSource audio in reportedAudios) {
      if (audio != null) {
        audio.Play();
      }
    }
    foreach (GameObject p in players) {
      meetingAudios.Add(p.transform.Find("MeetingAudio").GetComponent<AudioSource>());
    }
    foreach (AudioSource audio in meetingAudios) {
      if (audio != null) {
        audio.Play();
      }
    }
    foreach (GameObject p in players) {
      bgmAudios.Add(p.transform.Find("PlayerMusic").GetComponent<AudioSource>());
    }
    foreach (AudioSource audio in bgmAudios) {
      if (audio != null) {
        audio.Stop();
      }
    }
    Debug.Log("Hello my name jay");
  }

  public void TakeHit() => pv.RPC("RPC_TakeHit", RpcTarget.All);

  [PunRPC]
  public void RPC_TakeHit() {
    if (!pv.IsMine) return;
    minigameInterrupt = true;
    mapMan.ResetMap();
    miniMan.ResetTaskList();
    playerMan.Die();
  }

  void CurrentlyInVent() {
    if (Input.GetKeyDown("space")) {
      if ((transform.position - Vent1Pos.transform.position).sqrMagnitude < threshold) {
        transform.position = Vent2Pos.transform.position;
      } else if ((transform.position - Vent2Pos.transform.position).sqrMagnitude < threshold) {
        transform.position = Vent3Pos.transform.position;
      } else if ((transform.position - Vent3Pos.transform.position).sqrMagnitude < threshold) {
        transform.position = Vent1Pos.transform.position;
      }

    } else if (Input.GetKeyDown("a") || Input.GetKeyDown("w") || Input.GetKeyDown("d") || Input.GetKeyDown("s")) {
      pv.RPC("stopInvis", RpcTarget.All);
            //     pv.RPC("VentSound", RpcTarget.All, interactable.name);

            inVent = false;
    }
  }

  void EnterVent(Interactable interactable) {
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

  public void exitMinigame(bool exiting) {
    cam.SetActive(exiting);
    if (minimap != null) minimap.SetActive(exiting);
    if (GetComponent<Role>().subRole == Role.Roles.Assassin || GetComponent<Role>().subRole == Role.Roles.Chameleon) reticle.SetActive(exiting);
    sun.SetActive(exiting);
    GetComponents<PlayMakerFSM>()[0].enabled = exiting;
    GetComponent<PlayMakerFixedUpdate>().enabled = exiting;
    GetComponent<PlayMakerLateUpdate>().enabled = exiting;
  }

}