// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace HutongGames.PlayMaker.Pun2.Actions
{
    [Serializable]
    public class PlayerReferenceProperty
    {

        public enum PlayerReferences {localPlayer,MasterClient,ByNickName,ByActorNumber,ByUserId,next,ByRoomNumber,ByOwnedObject};

        public PlayerReferences reference;

        [Tooltip("The player nickname")]
        public FsmString nickname;

        public FsmInt actorNumber;

        public FsmString userId;

        public FsmInt roomNumber;

        public FsmOwnerDefault gameObject;

        public FsmEvent playerNotFound;

      
        public PlayerReferenceProperty()
        {
            reference = PlayerReferences.localPlayer;
            nickname = new FsmString() { UseVariable = true };

            actorNumber = new FsmInt() { UseVariable = true };
            userId = new FsmString() { UseVariable = true };
            roomNumber = new FsmInt() { UseVariable = true };
            gameObject = null;
        }

        public Player GetPlayer(FsmStateAction action)
        {
            Player _player = null;

            switch(reference)
            {
                case PlayerReferences.localPlayer:
                    _player = PhotonNetwork.LocalPlayer;
                    break;
                case PlayerReferences.MasterClient:
                    _player = PhotonNetwork.MasterClient;
                    break;
                case PlayerReferences.ByNickName:
                    _player = PhotonNetwork.CurrentRoom == null ? null :  PhotonNetwork.CurrentRoom.FindPlayerByNickname(nickname.Value);
                    break;
                case PlayerReferences.ByActorNumber:
                    _player =  PhotonNetwork.LocalPlayer.Get(actorNumber.Value);
                    break;
                case PlayerReferences.ByUserId:
                    _player = PhotonNetwork.LocalPlayer.FindByUserID(userId.Value);
                    break;
                case PlayerReferences.next:
                    _player =  PhotonNetwork.LocalPlayer.GetNext();
                    break;
                case PlayerReferences.ByRoomNumber:
                    _player = PhotonNetwork.CurrentRoom.FindPlayerByNumber(roomNumber.Value);
                    break;
                case PlayerReferences.ByOwnedObject:

                    GameObject _go = action.Fsm.GetOwnerDefaultTarget(gameObject);
                    if (_go != null)
                    {
                        PhotonView _pv = _go.GetComponent<PhotonView>();

                        if (_pv != null)
                        {
                            _player = _pv.Owner;
                        }
                    }
                    break;
            }

            if (_player == null)
            {
                action.Fsm.Event(playerNotFound);
            }

            return _player;
        }
    }
}
