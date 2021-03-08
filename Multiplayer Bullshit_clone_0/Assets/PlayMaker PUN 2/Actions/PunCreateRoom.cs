// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Create a room with given title. This will fail if the room title is already in use.\n" +
		"Note: If you don't want to create a unique room-name yourself, leave the room name to empty, and the server will assign a roomName (a GUID as string).")]
	[HelpUrl("")]
	public class PunCreateRoom : PunActionBase
	{
		[Tooltip("The room Name")]
		public FsmString roomName;
		
		[Tooltip("Is the room visible")]
		public FsmBool isVisible;
		
		[Tooltip("Is the room open")]
		public FsmBool isOpen;
			
		[Tooltip("Max numbers of players for this room.")]
		[TitleAttribute("Max Number Of Players")]
		public FsmInt maxNumberOfPLayers;

        public TypedLobbyProperty lobby;

        [ActionSection("Result")]
        [UIHint(UIHint.Variable)]
        [Tooltip("false if the room creation will not be attempted. True if it will attempt room creation")]
        public FsmBool result;

        [Tooltip("Event to send if the room creation will be attempted")]
        public FsmEvent willProceed;

        [Tooltip("Event to send if the room creation will not be attempted")]
        public FsmEvent willNotProceed;


        public override void Reset()
		{
			roomName  = new FsmString() { UseVariable = true };
			isVisible = true;
			isOpen = true;
            maxNumberOfPLayers = 4;

            lobby = new TypedLobbyProperty();
            result = null;
            willProceed = null;
            willNotProceed = null;
        }

		public override void OnEnter()
		{
			
			RoomOptions _options = new RoomOptions();
			_options.MaxPlayers =  (byte)maxNumberOfPLayers.Value;
			_options.IsVisible = isVisible.Value;
			_options.IsOpen = isOpen.Value;

            string _roomName = string.IsNullOrEmpty(roomName.Value)?null: roomName.Value;

            bool _result = PhotonNetwork.CreateRoom(_roomName, _options, lobby.GetTypedLobby());

            if (!result.IsNone)
            {
                result.Value = _result;
            }

            Fsm.Event(_result ? willProceed : willNotProceed);


            Finish();
		}

	}
}