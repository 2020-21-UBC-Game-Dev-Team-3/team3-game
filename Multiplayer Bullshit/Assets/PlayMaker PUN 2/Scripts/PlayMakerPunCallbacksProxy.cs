// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.

using System;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace HutongGames.PlayMaker.Pun2
{


    public class PunCallbackInfo
    {
        public Player Player;
        public PunCallbacks Callback;
        public string CallbackEvent;
    }
    
    /// <summary>
    /// Part of "PlayMaker Photon Callbacks Proxy" prefab.
    /// This behavior implements *All* messages from Photon, and broadcast associated global events.
    /// note: the instantiate call is featured in the PlayMakerPhotonGameObjectProxy component
    /// 
    /// The playmaker events corresponding to each Photon messages are declared in the Fsm named "Photon messages interface" in the "PlayMaker Photon Proxy" prefab.
    /// 
    /// Example: the photon message OnPhotonPlayerConnected (PhotonPlayer player) is translated as a global event "PHOTON / PHOTON PLAYER CONNECTED"
    /// the PhotonPlayer passed in these messages is stored in lastMessagePhotonPlayer and can be retrieved using the action "PhotonViewGetLastMessagePLayerProperties"
    /// 
    /// This behavior also watch the connection state and broadcast associated global events.
    /// example: PhotonNetwork.connectionState.Connecting is translated as a global event named "PHOTON / STATE : CONNECTING"
    public class PlayMakerPunCallbacksProxy : MonoBehaviourPunCallbacks
    {

        public static PlayMakerPunCallbacksProxy Instance;
        /// <summary>
        /// output in the console activities of the various elements.
        /// TODO: should be set to false for release
        /// </summary>
        public bool debug = true;


        private Stack<PunCallbackInfo> PunCallbacksStack = new Stack<PunCallbackInfo>();
        
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
        /// The last Pun callback.
        /// </summary>
        public PunCallbacks LastCallback = PunCallbacks.Unknown;

        /// <summary>
        /// The last PlayMaker event related to the last Pun2 callback;s
        /// </summary>
        public string LastCallbackEvent = string.Empty;

        /// <summary>
        /// The last disconnection or connection failure cause
        /// </summary>
        public DisconnectCause lastDisconnectCause;


        public Dictionary<string, object> lastCustomAuthenticationResponse;

        public static bool lastCreateRoomFailed = false;
        public short LastCreateRoomFailedReturnCode = 0;
        public string LastCreateRoomFailedMessage = string.Empty;

        public static bool lastJoinRoomFailed = false;
        public short LastJoinRoomFailedReturnCode = 0;
        public string LastJoinRoomFailedMessage = string.Empty;

        public static bool lastJoinRandomRoomFailed = false;
        public short LastJoinRandomRoomFailedReturnCode = 0;
        public string LastJoinRandomRoomFailedMessage = string.Empty;

        public List<FriendInfo> LastFriendList;

        public List<TypedLobbyInfo> lastlobbyStatistics; 

        public List<RoomInfo> LastRoomList;

        public ExitGames.Client.Photon.Hashtable LastRoomPropertiesThatChanged;

        public ExitGames.Client.Photon.Hashtable LastPlayerPropertiesUpdate;

        public ExitGames.Client.Photon.Hashtable LastRoomPropertiesUpdate;

        public RegionHandler LastRegionHandler;


        public OperationResponse LastWebRpcResponse;

        /// <summary>
        /// The last authentication failed debug message. Message is reseted when authentication is triggered again.
        /// </summary>
        public static string lastAuthenticationDebugMessage = string.Empty;

        /// <summary>
        /// Is True if the last authentication failed.
        /// </summary>
        public static bool lastAuthenticationFailed = false;


        const string DebugLabelPrefix = "<color=navy>PlayMaker Photon proxy: </color>";

        private void Awake()
        {
            Instance = this;
        }
        /// <summary>
        /// Watch connection state
        /// </summary>
        void Update()
        {
            Update_connectionStateWatcher();

            ProcessCallbackStack();
        }

        void ProcessCallbackStack()
        {
            if (PunCallbacksStack.Count == 0) return;
            
            PunCallbackInfo _callback = PunCallbacksStack.Pop();

            LastCallback = _callback.Callback;
            LastCallbackEvent = _callback.CallbackEvent;
            lastMessagePhotonPlayer = _callback.Player;

            BroadcastCallback();
        }

        #region Public Interface

        public int RoomListCount
        {
            get
            {
                if (LastRoomList!=null)
                {
                    return LastRoomList.Count;
                }

                return -1;
            }
        }

        #endregion

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
                    Debug.Log(DebugLabelPrefix + "PhotonNetwork.NetworkClientState changed from '" + lastConnectionState + "' to '" + PhotonNetwork.NetworkClientState + "'");
                }

                lastConnectionState = PhotonNetwork.NetworkClientState;
                PlayMakerFSM.BroadcastEvent(PlayMakerPunLUT.ClientStateEnumEvents[PhotonNetwork.NetworkClientState]);
            }

        }// Update_connectionStateWatcher

        #endregion


        #region Photon Messages Internal Handling

        /// <summary>
        /// The last callback data debug.
        /// Cached to avoid allocations
        /// </summary>
        Dictionary<string, string> _LastCallbackDataDebug = new Dictionary<string, string>();

        /// <summary>
        /// Broadcasts the Last callback.
        /// 
        /// </summary>
        void BroadcastCallback()
        {
            PlayMakerPhotonProxy.LogEventBroadcasting(DebugLabelPrefix,LastCallback.ToString(), LastCallbackEvent);

            PlayMakerFSM.BroadcastEvent(LastCallbackEvent);
        }
        
        #endregion

        #region Photon Messages

        public override void OnConnected()
        {
            LastCallback = PunCallbacks.OnConnected;
            LastCallbackEvent = PlayMakerPunLUT.CallbacksEvents[LastCallback];

            BroadcastCallback();
        }

        public override void OnConnectedToMaster()
        {
            LastCallback = PunCallbacks.OnConnectedToMaster;
            LastCallbackEvent = PlayMakerPunLUT.CallbacksEvents[LastCallback];

            BroadcastCallback();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            LastCallback = PunCallbacks.OnDisconnected;
            LastCallbackEvent = PlayMakerPunLUT.CallbacksEvents[LastCallback];

            lastDisconnectCause = cause;

            _LastCallbackDataDebug.Clear();
            _LastCallbackDataDebug.Add("Cause", cause.ToString());

            BroadcastCallback();
        }

        public override void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            LastCallback = PunCallbacks.OnCustomAuthenticationResponse;
            LastCallbackEvent = PlayMakerPunLUT.CallbacksEvents[LastCallback];

            lastAuthenticationFailed = false;
            lastCustomAuthenticationResponse = data;

            _LastCallbackDataDebug.Clear();

            foreach (var _d in data)
            {
                _LastCallbackDataDebug.Add(_d.Key, _d.Value.ToString());
            }

            BroadcastCallback();
        }

        public override void OnCustomAuthenticationFailed(string debugMessage)
        {
            LastCallback = PunCallbacks.OnCustomAuthenticationFailed;
            LastCallbackEvent = PlayMakerPunLUT.CallbacksEvents[LastCallback];

            lastAuthenticationDebugMessage = debugMessage;
            lastAuthenticationFailed = true;

            _LastCallbackDataDebug.Clear();
            _LastCallbackDataDebug.Add("Message", debugMessage);

            BroadcastCallback();
        }


        public override void OnJoinedLobby()
        {
            LastCallback = PunCallbacks.OnJoinedLobby;
            LastCallbackEvent = PlayMakerPunLUT.CallbacksEvents[LastCallback];

            BroadcastCallback();
        }

        public override void OnLeftLobby()
        {
            LastCallback = PunCallbacks.OnLeftLobby;
            LastCallbackEvent = PlayMakerPunLUT.CallbacksEvents[LastCallback];

            BroadcastCallback();
        }


        public override void OnCreatedRoom()
        {
            LastCallback = PunCallbacks.OnCreatedRoom;
            LastCallbackEvent = PlayMakerPunLUT.CallbacksEvents[LastCallback];

            lastCreateRoomFailed = false;
            LastCreateRoomFailedMessage = string.Empty;
            LastCreateRoomFailedReturnCode = 0;

            BroadcastCallback();
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            LastCallback = PunCallbacks.OnCreateRoomFailed;
            LastCallbackEvent = PlayMakerPunLUT.CallbacksEvents[LastCallback];

            lastCreateRoomFailed = true;
            LastCreateRoomFailedReturnCode = returnCode;
            LastCreateRoomFailedMessage = message;

            _LastCallbackDataDebug.Clear();
            _LastCallbackDataDebug.Add("ReturnCode", returnCode.ToString());
            _LastCallbackDataDebug.Add("Message", message);

            BroadcastCallback();
        }


        public override void OnJoinedRoom()
        {
            LastCallback = PunCallbacks.OnJoinedRoom;
            LastCallbackEvent = PlayMakerPunLUT.CallbacksEvents[LastCallback];

            lastJoinRoomFailed = false;
            LastJoinRoomFailedReturnCode = 0;
            LastJoinRoomFailedMessage = string.Empty;

            lastJoinRandomRoomFailed = false;
            LastJoinRandomRoomFailedReturnCode = 0;
            LastJoinRandomRoomFailedMessage = string.Empty;

            BroadcastCallback();
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            LastCallback = PunCallbacks.OnJoinRoomFailed;
            LastCallbackEvent = PlayMakerPunLUT.CallbacksEvents[LastCallback];

            lastJoinRoomFailed = true;
            LastJoinRoomFailedReturnCode = returnCode;
            LastJoinRoomFailedMessage = message;

            _LastCallbackDataDebug.Clear();
            _LastCallbackDataDebug.Add("ReturnCode", returnCode.ToString());
            _LastCallbackDataDebug.Add("Message", message);

            BroadcastCallback();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            LastCallback = PunCallbacks.OnJoinRandomFailed;
            LastCallbackEvent = PlayMakerPunLUT.CallbacksEvents[LastCallback];

            lastJoinRandomRoomFailed = true;
            LastJoinRandomRoomFailedReturnCode = returnCode;
            LastJoinRandomRoomFailedMessage = message;

            _LastCallbackDataDebug.Clear();
            _LastCallbackDataDebug.Add("ReturnCode", returnCode.ToString());
            _LastCallbackDataDebug.Add("Message", message);

            BroadcastCallback();
        }

        public override void OnLeftRoom()
        {
            LastCallback = PunCallbacks.OnLeftRoom;
            LastCallbackEvent = PlayMakerPunLUT.CallbacksEvents[LastCallback];

            BroadcastCallback();
        }
        

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            PunCallbackInfo _c = new PunCallbackInfo();
            _c.Player = newPlayer;
            _c.Callback = PunCallbacks.OnPlayerEnteredRoom;
            _c.CallbackEvent = PlayMakerPunLUT.CallbacksEvents[ _c.Callback];
            
            PunCallbacksStack.Push(_c);
            
            /*
            LastCallback = PunCallbacks.OnPlayerEnteredRoom;
            LastCallbackEvent = PlayMakerPunLUT.CallbacksEvents[LastCallback];

            lastMessagePhotonPlayer = newPlayer;

            _LastCallbackDataDebug.Clear();
            _LastCallbackDataDebug.Add("New Player", lastMessagePhotonPlayer.ToStringFull());

            
            BroadcastCallback();
            */
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            PunCallbackInfo _c = new PunCallbackInfo();
            _c.Player = otherPlayer;
            _c.Callback = PunCallbacks.OnPlayerLeftRoom;
            _c.CallbackEvent = PlayMakerPunLUT.CallbacksEvents[ _c.Callback];
            
            PunCallbacksStack.Push(_c);
            /*
            LastCallback = PunCallbacks.OnPlayerLeftRoom;
            LastCallbackEvent = PlayMakerPunLUT.CallbacksEvents[LastCallback];

            lastMessagePhotonPlayer = otherPlayer;

            _LastCallbackDataDebug.Clear();
            _LastCallbackDataDebug.Add("Other Player", lastMessagePhotonPlayer.ToStringFull());

            BroadcastCallback();
            */
        }

        public override void OnFriendListUpdate(List<FriendInfo> friendList)
        {
            LastCallback = PunCallbacks.OnFriendListUpdate;
            LastCallbackEvent = PlayMakerPunLUT.CallbacksEvents[LastCallback];

            LastFriendList = friendList;

            _LastCallbackDataDebug.Clear();
            _LastCallbackDataDebug.Add("Friend List", LastFriendList.ToStringFull());

            BroadcastCallback();
        }

        public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            LastCallback = PunCallbacks.OnLobbyStatisticsUpdate;
            LastCallbackEvent = PlayMakerPunLUT.CallbacksEvents[LastCallback];

            lastlobbyStatistics = lobbyStatistics;

            _LastCallbackDataDebug.Clear();
            _LastCallbackDataDebug.Add("Lobby List", lastlobbyStatistics.ToStringFull());

            BroadcastCallback();
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            LastCallback = PunCallbacks.OnMasterClientSwitched;
            LastCallbackEvent = PlayMakerPunLUT.CallbacksEvents[LastCallback];

            lastMessagePhotonPlayer = newMasterClient;

            _LastCallbackDataDebug.Clear();
            _LastCallbackDataDebug.Add("New MasterClient", lastMessagePhotonPlayer.ToStringFull());

            BroadcastCallback();
        }

        public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
        {
            LastCallback = PunCallbacks.OnRoomListUpdate;
            LastCallbackEvent = PlayMakerPunLUT.CallbacksEvents[LastCallback];

            lastMessagePhotonPlayer = target;
            LastPlayerPropertiesUpdate = changedProps;

            _LastCallbackDataDebug.Clear();
            _LastCallbackDataDebug.Add("Player", target.ToStringFull());
            _LastCallbackDataDebug.Add("Properties that changed", changedProps.ToStringFull());

            BroadcastCallback();
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            LastCallback = PunCallbacks.OnRoomListUpdate;
            LastCallbackEvent = PlayMakerPunLUT.CallbacksEvents[LastCallback];

            LastRoomList = roomList;

            _LastCallbackDataDebug.Clear();
            _LastCallbackDataDebug.Add("Roomlist", LastRoomList.ToStringFull());

            BroadcastCallback();
        }

        public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
        {
            LastCallback = PunCallbacks.OnRoomPropertiesUpdate;
            LastCallbackEvent = PlayMakerPunLUT.CallbacksEvents[LastCallback];

            LastRoomPropertiesThatChanged = propertiesThatChanged;

            _LastCallbackDataDebug.Clear();
            _LastCallbackDataDebug.Add("Properties that changed", LastRoomPropertiesThatChanged.ToStringFull());

            BroadcastCallback();
        }

        public override void OnRegionListReceived(RegionHandler regionHandler)
        {
            LastCallback = PunCallbacks.OnRegionListReceived;
            LastCallbackEvent = PlayMakerPunLUT.CallbacksEvents[LastCallback];

            LastRegionHandler = regionHandler;

            _LastCallbackDataDebug.Clear();
            _LastCallbackDataDebug.Add("Summary", LastRegionHandler.SummaryToCache);

            BroadcastCallback();
        }

        public override void OnWebRpcResponse(OperationResponse response)
        {
            LastCallback = PunCallbacks.OnWebRpcResponse;
            LastCallbackEvent = PlayMakerPunLUT.CallbacksEvents[LastCallback];

            LastWebRpcResponse = response;

            _LastCallbackDataDebug.Clear();
            _LastCallbackDataDebug.Add("Response", LastWebRpcResponse.ToStringFull());

            BroadcastCallback();

        }



        //void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
        //{
        //    Player player = playerAndUpdatedProps[0] as Player;
        //    //Hashtable props = playerAndUpdatedProps[1] as Hashtable;

        //    if (debug)
        //    {
        //        Debug.Log("PLayMaker Photon proxy:OnPhotonPlayerPropertiesChanged:" + player);
        //    }

        //    lastMessagePhotonPlayer = player;

        //    PlayMakerFSM.BroadcastEvent("PHOTON / PLAYER PROPERTIES CHANGED");
        //}




        #endregion


        #region OwnerShip Request

        //public void OnOwnershipRequest(object[] viewAndPlayer)
        //{
        //    PhotonView view = viewAndPlayer[0] as PhotonView;
        //    Player requestingPlayer = viewAndPlayer[1] as Player;

        //    if (debug)
        //    {
        //        Debug.Log("OnOwnershipRequest(): Player " + requestingPlayer + " requests ownership of: " + view + ".");
        //    }

        //    view.TransferOwnership(requestingPlayer.ActorNumber);
        //}

        #endregion

        #region utils

        // this is a fix to the regular BroadCast that for some reason delivers events too late when player joins a room

        public static void BroadCastToAll(string fsmEvent)
        {

            var list = new List<PlayMakerFSM>(PlayMakerFSM.FsmList);

            foreach (PlayMakerFSM fsm in list)
            {

                //if (true)
                //{
                fsm.SendEvent(fsmEvent); // fine
                                         //}else{	
                                         //	fsm.Fsm.ProcessEvent(FsmEvent.GetFsmEvent(fsmEvent)); // too late. This is the PlayMakerFsm.BroadcastEvent() way as far as I can tell.
                                         //}
            }

        }



        #endregion
    }
}