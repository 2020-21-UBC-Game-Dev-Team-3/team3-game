using System.Collections.Generic;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Pun2
{

    public enum PunCallbacks
    {
        OnConnected                             = 1,
        OnConnectedToMaster                     = 2,
        OnDisconnected                          = 3,
        OnCustomAuthenticationResponse          = 4,
        OnCustomAuthenticationFailed            = 5,
        OnJoinedLobby                           = 6,
        OnLeftLobby                             = 7,
        OnCreatedRoom                           = 8,
        OnCreateRoomFailed                      = 9,
        OnJoinedRoom                            = 10,
        OnJoinRoomFailed                        = 11,
        OnJoinRandomFailed                      = 12,
        OnLeftRoom                              = 13,
        OnPlayerEnteredRoom                     = 14,
        OnPlayerLeftRoom                        = 15,
        OnFriendListUpdate                      = 16,
        OnRoomListUpdate                        = 17,
        OnRoomPropertiesUpdate                  = 18,
        OnPlayerPropertiesUpdate                = 19,
        OnLobbyStatisticsUpdate                 = 20,
        OnMasterClientSwitched                  = 21,
        OnRegionListReceived                    = 22,
        OnWebRpcResponse                        = 23,
        OnPhotonInstantiate                     = 24,
        Unknown                                 = 0
    }

    public enum PunCallbacksWithData
    {
        OnDisconnected                          = 3,
        OnCustomAuthenticationResponse          = 4,
        OnCustomAuthenticationFailed            = 5,
        OnCreateRoomFailed                      = 8,
        OnJoinRoomFailed                        = 11,
        OnJoinRandomFailed                      = 12,
        OnPlayerEnteredRoom                     = 14,
        OnPlayerLeftRoom                        = 15

    }

    public class PlayMakerPunLUT
    {
        public static readonly Dictionary<PunCallbacks, string> CallbacksEvents = new Dictionary<PunCallbacks, string>()
    {
        {PunCallbacks.OnConnected,                     "PHOTON / ON CONNECTED"},
        {PunCallbacks.OnConnectedToMaster,             "PHOTON / ON CONNECTED TO MASTER"},
        {PunCallbacks.OnDisconnected,                  "PHOTON / ON DISCONNECTED"},
        {PunCallbacks.OnCustomAuthenticationResponse,  "PHOTON / ON CUSTOM AUTHENTICATION RESPONSE"},
        {PunCallbacks.OnCustomAuthenticationFailed,    "PHOTON / ON CUSTOM AUTHENTICATION FAILED"},
        {PunCallbacks.OnJoinedLobby,                   "PHOTON / ON JOINED LOBBY"},
        {PunCallbacks.OnLeftLobby,                     "PHOTON / ON LEFT LOBBY"},
        {PunCallbacks.OnCreatedRoom,                   "PHOTON / ON CREATED ROOM"},
        {PunCallbacks.OnCreateRoomFailed,              "PHOTON / ON CREATE ROOM FAILED "},
        {PunCallbacks.OnJoinedRoom,                    "PHOTON / ON JOINED ROOM"},
        {PunCallbacks.OnJoinRoomFailed,                "PHOTON / ON JOINED ROOM FAILED"},
        {PunCallbacks.OnJoinRandomFailed,              "PHOTON / ON JOIN RANDOM ROOM FAILED"},
        {PunCallbacks.OnLeftRoom,                      "PHOTON / ON LEFT ROOM"},
        {PunCallbacks.OnPlayerEnteredRoom,             "PHOTON / ON PLAYER ENTERED ROOM"},
        {PunCallbacks.OnPlayerLeftRoom,                "PHOTON / ON PLAYER LEFT ROOM"},
        {PunCallbacks.OnFriendListUpdate,              "PHOTON / ON FRIEND LIST UPDATE"},
        {PunCallbacks.OnRoomListUpdate,                "PHOTON / ON ROOM LIST UPDATE"},
        {PunCallbacks.OnRoomPropertiesUpdate,          "PHOTON / ON ROOM PROPERTIES UPDATE"},
        {PunCallbacks.OnPlayerPropertiesUpdate,        "PHOTON / ON PLAYER PROPERTIES UPDATE"},
        {PunCallbacks.OnLobbyStatisticsUpdate,         "PHOTON / ON LOBBY STATISTICS UPDATE"},
        {PunCallbacks.OnMasterClientSwitched,          "PHOTON / ON MASTERCLIENT SWITCHED"},
        {PunCallbacks.OnRegionListReceived,            "PHOTON / ON REGION LIST RECEIVED"},
        {PunCallbacks.OnWebRpcResponse,                "PHOTON / ON WEBRPC RESPONSE"},
        {PunCallbacks.OnPhotonInstantiate,             "PHOTON / ON PHOTON INSTANTIATE"}
    };


        public static readonly Dictionary<ClientState, string> ClientStateEnumEvents = new Dictionary<ClientState, string>()
    {
        {ClientState.Authenticated,                     "PHOTON / CLIENT STATE / AUTHENTICATED"},
        {ClientState.Authenticating,                    "PHOTON / CLIENT STATE / AUTHENTICATING"},
        {ClientState.ConnectedToGameServer,             "PHOTON / CLIENT STATE / CONNECTED TO GAMESERVER"},
        {ClientState.ConnectedToMasterServer,           "PHOTON / CLIENT STATE / CONNECTED TO MASTERSERVER"},
        {ClientState.ConnectedToNameServer,             "PHOTON / CLIENT STATE / CONNECTED TO NAMESERVER"},
        {ClientState.ConnectingToGameServer,            "PHOTON / CLIENT STATE / CONNECTING TO GAMESERVER"},
        {ClientState.ConnectingToNameServer,            "PHOTON / CLIENT STATE / CONNECTING TO NAMESERVER"},
        {ClientState.ConnectingToMasterServer,          "PHOTON / CLIENT STATE / CONNECTING TO MASTERSERVER"},
        {ClientState.Disconnected,                      "PHOTON / CLIENT STATE / DISCONNECTED"},
        {ClientState.Disconnecting,                     "PHOTON / CLIENT STATE / DISCONNECTING"},
        {ClientState.DisconnectingFromGameServer,       "PHOTON / CLIENT STATE / DISCONNECTING FROM GAMESERVER"},
        {ClientState.DisconnectingFromMasterServer,     "PHOTON / CLIENT STATE / DISCONNECTING FROM MASTERSERVER"},
        {ClientState.DisconnectingFromNameServer,       "PHOTON / CLIENT STATE / DISCONNECTING FROM NAMESERVER"},
        {ClientState.Joined,                            "PHOTON / CLIENT STATE / JOINED"},
        {ClientState.JoinedLobby,                       "PHOTON / CLIENT STATE / JOINED LOBBY"},
        {ClientState.Joining,                           "PHOTON / CLIENT STATE / JOINING"},
        {ClientState.JoiningLobby,                      "PHOTON / CLIENT STATE / JOINING LOBBY"},
        {ClientState.Leaving,                           "PHOTON / CLIENT STATE / LEAVING"},
        {ClientState.PeerCreated,                       "PHOTON / CLIENT STATE / PEER CREATED"}
    };


        static List<string> _photonEvents;

        public static List<string> PhotonEvents
        {
            get
            {
                if (_photonEvents == null)
                {
                    _photonEvents = new List<string>();
                    _photonEvents.AddRange(ClientStateEnumEvents.Values);
                    _photonEvents.AddRange(CallbacksEvents.Values);

                }

                return _photonEvents;
            }
        }

    }
}