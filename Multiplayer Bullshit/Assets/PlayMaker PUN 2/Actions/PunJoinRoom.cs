// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Join room with given title. Global Photon Event 'PUN / ON JOINED ROOM' will occur, or 'PHOTON / ON CREATED ROOM' if createIfnotExists was true and processed. If no such room exists and createIfnotExists is set to false, An Photon Error Event will occur 'FAILED TO JOIN ROOM'")]
	[HelpUrl("")]
	public class PunJoinRoom : PunActionBase
    {
		[Tooltip("The room Name")]
		public FsmString roomName;
		
		[Tooltip("If true, the server will attempt to create a room, as if CreateRoom was called instead.")]
		public FsmBool createIfNotExists;

        public TypedLobbyProperty lobby;

        [ActionSection("Result")]
        [UIHint(UIHint.Variable)]
        [Tooltip("false if the request will not be attempted. True will attempt request")]
        public FsmBool result;

        [Tooltip("Event to send if the request will be attempted")]
        public FsmEvent willProceed;

        [Tooltip("Event to send if the request will not be attempted")]
        public FsmEvent willNotProceed;

        public override void Reset()
		{
			roomName  = null;
			createIfNotExists = false;
            lobby = new TypedLobbyProperty();

            result = null;
            willProceed = null;
            willNotProceed = null;

        }

		public override void OnEnter()
		{
            bool _result;

			if (createIfNotExists.Value)
			{
				_result = PhotonNetwork.JoinOrCreateRoom(roomName.Value,new RoomOptions(),lobby.GetTypedLobby());
			}else{
				_result = PhotonNetwork.JoinRoom(roomName.Value);
			}

            if (!result.IsNone)
            {
                result.Value = _result;
            }

            Fsm.Event(_result ? willProceed : willNotProceed);

            Finish();
		}

	}
}