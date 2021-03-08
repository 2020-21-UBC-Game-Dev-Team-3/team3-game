// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Leave a lobby to stop getting updates about available rooms." +
		"This does not reset PhotonNetwork.lobby! This allows you to join this particular lobby later easily." +
		"" +
		"The values countOfPlayers, countOfPlayersOnMaster, countOfPlayersInRooms and countOfRooms" +
		"are received even without being in a lobby." +
		"" +
		"You can use JoinRandomRoom without being in a lobby.")]
	[HelpUrl("")]
	public class PunJoinLobby : PunActionBase
	{

		[Tooltip("The Lobby of the room to join")]
        public TypedLobbyProperty lobby;

        [ActionSection("Result")]
        [UIHint(UIHint.Variable)]
        [Tooltip("false if the lobby will not be joined")]
        public FsmBool result;


        [Tooltip("Event to send if joining the lobby will be attempted")]
        public FsmEvent willProceed;

        [Tooltip("Event to send if there will be no attempt to join the lobby")]
        public FsmEvent willNotProceed;



        public override void Reset()
		{
            lobby = new TypedLobbyProperty();
            result = null;
			willProceed = null;
			willNotProceed = null;
		}


		public override void OnEnter()
		{
            bool _result = false;

            _result = PhotonNetwork.JoinLobby(lobby.GetTypedLobby());

			if (!result.IsNone)
			{
				result.Value = _result;
			}
			
			Fsm.Event(_result ? willProceed : willNotProceed);

			Finish();
		}
	}
}