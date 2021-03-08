// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License


using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Pun2
{
    /// <summary>
    /// Part of "PlayMaker Photon Proxy" prefab.
    /// 
    /// This behavior watches for FsmVariables being synched over the network ( FsmVariables with the check box "network sync" on), and verify that
    /// the correct setup is implemented for the network synchronization to work, that is:
    /// First  : a PlaymakerPhotonView needs to observe a fsm component with at least onefsm variable with network sync on. 
    /// Second : this PlaymakerPhotonView is in turn observed by a PhotonView.
    /// 
    /// 
    /// TODO: To implement The "network sync" check box properly:
    /// 
    ///  -- FSMString not supported, and other types --> Photon supports any objects. 
    ///  -- Fsm Global vars not supported.
    ///  
    ///  -- playmaker api enhancment?: 
    ///  -- flag for a fsm if it contains at least one fsmVariable to synch over netork, or better the list of variable that synch.
    ///  -- some kind of common class to work with fsmvars instead of duplicate 12 times code for each type of fsmvariable ( or a c# trick)
    /// 
    /// </summary>
    public class PlayMakerPhotonProxy : MonoBehaviourPunCallbacks
    {

        public static PlayMakerPhotonProxy Instance;

        /// <summary>
        /// output in the console activities of the various elements.
        /// TODO: should be set to false for release
        /// </summary>
        public bool debug = true;

        /// <summary>
        /// The last state of the connection. This is used to watch connection state changes and broadcast related Events.
        /// So you can receive an event when the connection is "disconnecting" or "Connecting", something not available as messages.
        /// TOREVIEW: this is a goodie, but very useful within playmaker environment, it's easier and more adequate then watching for the connection state within a fsm.
        /// </summary>
        private ClientState lastConnectionState;

        /// <summary>
        /// The photon player sent with a message like OnPhotonPlayerConnected, OnPhotonPlayerDisconnected or OnMasterClientSwitched
        /// Only the last instance is stored. Use PhotonNetworkGetMessagePlayerProperties Action to retrieve it within PlayMaker.
        /// This also store the player from the photonMessageinfo of the RPC calls implemented in this script.
        /// </summary>
        public Player lastMessagePhotonPlayer;



        /// <summary>
        /// The last authentication failed debug message. Message is reseted when authentication is triggered again.
        /// </summary>
        public static string lastAuthenticationDebugMessage = string.Empty;

        /// <summary>
        /// Is True if the last authentication failed.
        /// </summary>
        public static bool lastAuthenticationFailed = false;




        #region Photon network synch


        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
            }

            Instance = this;

            DontDestroyOnLoad(this.gameObject);
        }


        /// <summary>
        /// Used to check for fsm that required a photon view, and make sure it is set up properly: fsm<->playmakerPhotonView<->PhotonView
        /// </summary>
        void Start()
        {
            List<PlayMakerFSM> FsmToObserveList = GetFsmsWithNetworkSynchedVariables();

            //now for each of these Fsm check that a playmaker Photon gameObject proxy is attached to it, else will complain
            foreach (PlayMakerFSM fsm in FsmToObserveList)
            {
                PlayMakerPhotonGameObjectProxy goProxy = fsm.gameObject.GetComponent<PlayMakerPhotonGameObjectProxy>();

                if (goProxy == null)
                {
                    Debug.LogError("Missing PlayMakerPhotonGameObjectProxy on GameObject '" + fsm.gameObject.name + "' with Fsm '" + fsm.FsmName + "' containing variables supposed to be synched over the network");
                }

            }

            //Debug.Log(PingCloudRegions.closestRegion);

        }// start

        /// <summary>
        /// Logs the event broadcasting. Only if Debug is on
        /// </summary>
        /// <param name="punCallback">Pun callback.</param>
        /// <param name="fsmEvent">Fsm event.</param>
        /// <param name="eventData">Event data.</param>
       public static void LogEventBroadcasting(string source, string punCallback, string fsmEvent, Dictionary<string, string> eventData = null)
        {
            if (Instance.debug)
            {
                string _data = string.Empty;
                if (eventData != null)
                {
                    foreach (KeyValuePair<string, string> data in eventData)
                    {
                        _data += "<color=darkblue>" + data.Key + "</color>=<color=<darkblue>" + data.Value + "</color> ";
                    }
                }
                else
                {
                    _data = "No Data associated with this event";
                }

                Debug.Log(source+" Received Callback <color=fuchsia>" + punCallback + "</color> " +
                          "Broadcasting global Event <color=fuchsia>" + fsmEvent + "</color>\n" +
                          _data
                    , Instance);
            }
        }


        /// <summary>
        /// pre flight check on game object. Making sure it's set up properly to connect playmaker and PUN together.
        /// </summary>
        /// <returns>
        /// The pre flight check on game object.
        /// </returns>
        /// <param name='go'>
        /// true if pre flight check went ok: else something went wrong...
        /// </param>
        public bool ValidatePreFlightCheckOnGameObject(GameObject go)
        {
            if (go == null)
            {
                return false;
            }

            PlayMakerPhotonGameObjectProxy[] proxies = go.GetComponents<PlayMakerPhotonGameObjectProxy>();
            if (proxies.Length == 0)
            {
                Debug.LogError("Instanciating a GameObject with photon network require that you add a 'PlayMakerPhotonGameObjectProxy' component to the gameObject");
            }

            return true;
        }// ValidatePreFlightCheckOnGameObject

        /// <summary>
        /// Sanitizes the game object and check photonView observing fsm, else complain. 
        /// If photonView there, will insert PlayMakerPhotonView inbetween. 
        /// </summary>
        /// <param name='go'>
        /// Go.
        /// </param>
        public void SanitizeGameObject(GameObject go)
        {
            if (go == null)
            {
                return;
            }

            PhotonView[] allPhotonViews = go.GetComponentsInChildren<PhotonView>();

            // now make sure all fsm with network synchronized variable have a photonView attached
            List<PlayMakerFSM> fsmsToObserve = GetFsmsWithNetworkSynchedVariables(go);
            Debug.Log("found fsm to observe : " + fsmsToObserve.Count);
            foreach (PlayMakerFSM fsm in fsmsToObserve)
            {
                bool ok = false;
                foreach (PhotonView _photonView in allPhotonViews)
                {
                    foreach (Component _comp in _photonView.ObservedComponents)
                    {
                        if (_comp == fsm)
                        {
                            ok = true;
                            break;
                        }
                    }
                }

                if (!ok)
                {
                    Debug.LogError(
                        string.Format(
                            "Fsm component '{0}' on gameObject '{1}' has variable checked for network synching, but no PhotonView component is observing this fsm",
                            fsm.name,
                            fsm.gameObject.name)
                        );
                }

            }

            // now inject PlayMakerPhotonView where required.
            foreach (PhotonView _photonView in allPhotonViews)
            {
                Debug.Log(" photon view observing : "+_photonView.ObservedComponents.ToStringFull()+" "+_photonView.ViewID);

                Component _comp_i;
                for (int i = 0; i < _photonView.ObservedComponents.Count; i++)
                {
                    _comp_i = _photonView.ObservedComponents[i];
                    
                    if (_comp_i is PlayMakerFSM)
                    {
                        PlayMakerFSM fsm = (PlayMakerFSM)_comp_i;
                        PlayMakerPhotonView synchProxy = _photonView.gameObject.AddComponent<PlayMakerPhotonView>();
                        Debug.Log("switching observed <"+ i +">");
                        synchProxy.observed = fsm;

                        _photonView.ObservedComponents[i] = synchProxy;
                        
                    }

                    i++;
                }

                Debug.Log(" photon view observing job done : " + _photonView.ObservedComponents.ToStringFull() + " " + _photonView.ViewID);

            }

        }// SanitizeGameObject


        /// <summary>
        /// Gets the list of fsm from a given gameObject having network fsm synched variables.
        /// </summary>
        /// <returns>
        /// The list of fsm with network fsm synched variables on that gameObject.
        /// </returns>
        private List<PlayMakerFSM> GetFsmsWithNetworkSynchedVariables(GameObject go)
        {

            List<PlayMakerFSM> FsmToObserveList = new List<PlayMakerFSM>();

            PlayMakerFSM[] allFsms = go.GetComponentsInChildren<PlayMakerFSM>();


            foreach (PlayMakerFSM fsm in allFsms)
            {
                if (!FsmToObserveList.Contains(fsm))
                {
                    if (HasFsmNetworkingSynchVariables(fsm))
                    {
                        FsmToObserveList.Add(fsm);
                    }
                }
            }

            return FsmToObserveList;
        }// GetFsmsWithNetworkSynchedVariables



        /// <summary>
        /// Gets the list of fsm having network fsm synched variables.
        /// </summary>
        /// <returns>
        /// The list of fsm with network fsm synched variables.
        /// </returns>
        private List<PlayMakerFSM> GetFsmsWithNetworkSynchedVariables()
        {

            List<PlayMakerFSM> FsmToObserveList = new List<PlayMakerFSM>();

            foreach (PlayMakerFSM fsm in PlayMakerFSM.FsmList)
            {
                if (!FsmToObserveList.Contains(fsm))
                {
                    if (HasFsmNetworkingSynchVariables(fsm))
                    {
                        FsmToObserveList.Add(fsm);
                    }
                }
            }

            return FsmToObserveList;
        }// GetFsmsWithNetworkSynchedVariables


        /// <summary>
        /// Determines whether a given fsm has network synch variables.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this fsm features network synch variables; otherwise, <c>false</c>.
        /// </returns>
        /// <param name='fsm'>
        /// If set to <c>true</c> fsm.
        /// </param>
        private bool HasFsmNetworkingSynchVariables(PlayMakerFSM fsm)
        {
            foreach (FsmFloat fsmFloat in fsm.FsmVariables.FloatVariables)
            {
                if (fsmFloat.NetworkSync)
                {
                    return true;
                }
            }

            foreach (FsmInt fsmInt in fsm.FsmVariables.IntVariables)
            {
                if (fsmInt.NetworkSync)
                {
                    return true;
                }
            }

            foreach (FsmVector2 fsmVector2 in fsm.FsmVariables.Vector2Variables)
            {
                if (fsmVector2.NetworkSync)
                {
                    return true;

                }
            }

            foreach (FsmVector3 fsmVector3 in fsm.FsmVariables.Vector3Variables)
            {
                if (fsmVector3.NetworkSync)
                {
                    return true;

                }
            }

            foreach (FsmQuaternion fsmQuaternion in fsm.FsmVariables.QuaternionVariables)
            {
                if (fsmQuaternion.NetworkSync)
                {
                    return true;

                }
            }

            foreach (FsmColor fsmColor in fsm.FsmVariables.ColorVariables)
            {
                if (fsmColor.NetworkSync)
                {
                    return true;

                }
            }

            foreach (FsmBool fsmBool in fsm.FsmVariables.BoolVariables)
            {
                if (fsmBool.NetworkSync)
                {
                    return true;
                }
            }

            foreach (FsmString fsmString in fsm.FsmVariables.StringVariables)
            {
                if (fsmString.NetworkSync)
                {
                    return true;
                }
            }



            // TODO: duplicate for all Fsm variable types... yeekkk... could we not have a FsmVariableRoot extending the commong stuff or a c# trick?
            return false;
        }// HasFsmNetworkingSynchVariables


        /*
        /// <summary>
        /// Determines whether a PhotonView is observing the specified fsm.
        /// </summary>
        /// <returns>
        /// <c>true</c> if a PhotonView is observing the specified fsm; otherwise, <c>false</c>.
        /// </returns>
        /// <param name='fsm'>
        /// the fsm component
        /// </param>
        /// <param name='thePhotonView'>
        /// the photonView component IF it is observing fsm.
        /// </param>
        private PhotonView GetPhotonViewObservingFsm(PlayMakerFSM fsm)
        {
            Debug.Log("GetPhotonViewObservingFsm "+fsm.FsmName  );

            PhotonView[] photonViews = fsm.GetComponents<PhotonView>();
            foreach(PhotonView photonview in  photonViews)
            {

                //PlayMakerFSM observedFsm = (PlayMakerFSM)photonview.observed;
                //if (observedFsm!=null)
                //{
                //	Debug.Log(photonview.observed.GetType().ToString());
                //}
                if (photonview.observed == fsm){
                    return photonView;
                }
            }
            return null;
        }// GetPhotonViewObservingFsm
        */

        #endregion

        #region Photon RPC PLAYER
        /// <summary>
        /// output in the console photon message activity.
        /// TODO: should be set to false for release
        /// </summary>
        public bool LogMessageInfo = false;

        /// <summary>
        /// Function typically called from the action "PhotonViewRpcBroadcasFsmEvent" that use RPC to send information about the event to broadcast
        /// </summary>
        /// <param name='target'>
        /// Photon player Target.
        /// </param>
        /// <param name='globalEventName'>
        /// Global Fsm event name to broadcast to the player target
        /// </param>
        public void PhotonRpcBroadcastFsmEvent(Player target, string globalEventName)
        {
            if (LogMessageInfo)
            {
                Debug.Log("RPC to send global Fsm Event:" + globalEventName + " to player:" + target);
            }

            photonView.RPC("rpc", target, globalEventName);
        }

        /// <summary>
        /// Function typically called from the action "PhotonViewRpcBroadcasFsmEventToPlayer" that use RPC to send information about the event to broadcast
        /// </summary>
        /// <param name='target'>
        /// Photon player Target.
        /// </param>
        /// <param name='globalEventName'>
        /// Global Fsm event name to broadcast to the player target
        /// </param>
        /// <param name='stringData'>
        /// String data to pass with this event. WARNING: this is not supposed to be (nor efficient) a way to synchronize data. This is simply to comply with
        /// the ability for FsmEvent to include data.
        /// </param>
        public void PhotonRpcFsmBroadcastEventWithString(Player target, string globalEventName, string stringData)
        {
            if (LogMessageInfo)
            {
                Debug.Log("RPC to send string:" + stringData + " with global Fsm Event:" + globalEventName + " to player:" + target.ToString());
            }

            photonView.RPC("rpc_s", target, globalEventName, stringData);
        }

        #endregion


        #region Photon RPC TARGETS

        /// <summary>
        /// Function typically called from the action "PhotonViewRpcBroadcasFsmEvent" that use RPC to send information about the event to broadcast
        /// </summary>
        /// <param name='target'>
        /// Photon Target.
        /// </param>
        /// <param name='globalEventName'>
        /// Global Fsm event name to broadcast using the photon target rule.
        /// </param>
        public void PhotonRpcBroadcastFsmEvent(RpcTarget target, string globalEventName)
        {
            if (LogMessageInfo)
            {
                Debug.Log("RPC to send global Fsm Event:" + globalEventName + " to target:" + target,this);
            }

            this.photonView.RPC("rpc", target, globalEventName);// method name used to be too long : "RPC_PhotonRpcBroadcastFsmEvent"
        }

        /// <summary>
        /// Function typically called from the action "PhotonViewRpcBroadcasFsmEvent" that use RPC to send information about the event to broadcast
        /// </summary>
        /// <param name='target'>
        /// Photon Target.
        /// </param>
        /// <param name='globalEventName'>
        /// Global Fsm event name to broadcast using the photon target rule.
        /// </param>	
        /// <param name='stringData'>
        /// String data to pass with this event. WARNING: this is not supposed to be (nor efficient) a way to synchronize data. This is simply to comply with
        /// the ability for FsmEvent to include data.
        /// </param>
        public void PhotonRpcBroadcastFsmEventWithString(RpcTarget target, string globalEventName, string stringData)
        {
            if (LogMessageInfo)
            {
                Debug.Log("RPC to send string:" + stringData + "  with global Fsm Event:" + globalEventName + " to target:" + target);
            }

            photonView.RPC("rpc_s", target, globalEventName, stringData);// method name used to be too long :  "RPC_FsmPhotonRpcBroadcastFsmEventWithString"
        }


        #endregion

        #region Photon RPC TARGETS FUNCTIONS
        //-- TODO: more rpc Events signatures or a more verstatile signature perhaps? can't find a way tho...
        // at least a set of signature to provide support for all eventdata, but that means *A LOT* of signatures for all possible cases 
        // ( string,  string int, string float,  string int float, string int float vector3, etc...) overwhelming.
        //--

        /// <summary>
        /// RPC CALL. The paired rpc called triggered by PhotonRpcBroacastFsmEvent ( either by player or target)
        /// this will broadcast to All Fsm a global Fsm Event.
        /// The sender properties is accessible using the action "PhotonViewGetLastMessagePLayerProperties"
        /// </summary>
        /// <param name='globalEventName'>
        /// Global Fsm event name.
        /// </param>
        /// <param name='info'>
        /// Info.
        /// </param>
        [PunRPC]
        void rpc(string globalEventName, PhotonMessageInfo info) // method name used to be too long :  RPC_PhotonRpcBroadcastFsmEvent
        {
            if (LogMessageInfo)
            {
                Debug.Log("RPC Received for global event '"+ globalEventName+ "' info:"+info,this);
            }
            
            lastMessagePhotonPlayer = info.Sender;

            PlayMakerFSM.BroadcastEvent(globalEventName);
        }

        /// <summary>
        /// RPC CALL. The paired rpc called triggered by PhotonRpcBroacastFsmEventWithString ( either by player or target)
        /// this will broadcast to All Fsm a global Fsm Event.
        /// The sender properties is accessible using the action "PhotonViewGetLastMessagePLayerProperties"
        /// </summary>
        /// <param name='globalEventName'>
        /// Global Fsm event name.
        /// </param>
        /// <param name='info'>
        /// Info.
        /// </param>
        [PunRPC]
        void rpc_s(string globalEventName, string stringData, PhotonMessageInfo info)// method name used to be too long : RPC_FsmPhotonRpcBroadcastFsmEventWithString
        {
            if (LogMessageInfo)
            {
                Debug.Log(info.Sender.NickName + " sent RPC string:" + stringData + " from Fsm Event:" + globalEventName);
            }

            lastMessagePhotonPlayer = info.Sender;

            Fsm.EventData.StringData = stringData;

            PlayMakerFSM.BroadcastEvent(globalEventName);
        }

        #endregion

        /// <summary>
        /// Watch connection state
        /// </summary>
        void Update()
        {
            Update_connectionStateWatcher();
        }

        #region connection state watcher

        /// <summary>
        /// Watch connection state and broadcast associated FsmEvent.
        /// </summary>
        private void Update_connectionStateWatcher()
        {

            if (lastConnectionState != PhotonNetwork.NetworkClientState)
            {
                if (debug)
                {
                    Debug.Log("PhotonNetwork.NetworkClientState changed from '" + lastConnectionState + "' to '" + PhotonNetwork.NetworkClientState + "'");
                }

                lastConnectionState = PhotonNetwork.NetworkClientState;
                PlayMakerFSM.BroadcastEvent(PlayMakerPunLUT.ClientStateEnumEvents[PhotonNetwork.NetworkClientState]);
            }

        }// Update_connectionStateWatcher

        #endregion


        #region OwnerShip Request

        public void OnOwnershipRequest(object[] viewAndPlayer)
        {
            PhotonView view = viewAndPlayer[0] as PhotonView;
            Player requestingPlayer = viewAndPlayer[1] as Player;

            if (debug)
            {
                Debug.Log("OnOwnershipRequest(): Player " + requestingPlayer + " requests ownership of: " + view + ".");
            }

            view.TransferOwnership(requestingPlayer.ActorNumber);
        }

        #endregion

    }
}