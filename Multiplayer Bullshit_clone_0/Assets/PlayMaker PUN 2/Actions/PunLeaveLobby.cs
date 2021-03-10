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
	public class PunLeaveLobby : PunActionBase
	{
		[ActionSection("Result")]
		[UIHint(UIHint.Variable)]
		[Tooltip("false if there is no known room or game server to return to. True will attempt reconnection")]
		public FsmBool result;
		
		[Tooltip("Event to send if the reconnection will be attempted")]
		public FsmEvent willProceed;
		
		[Tooltip("Event to send if there is no known room or game server to return to")]
		public FsmEvent willNotProceed;

		public override void Reset()
		{
			result = null;
			willProceed = null;
			willNotProceed = null;
		}


		public override void OnEnter()
		{
			bool _result = PhotonNetwork.LeaveLobby();

			if (!result.IsNone)
			{
				result.Value = _result;
			}
			
			Fsm.Event(_result ? willProceed : willNotProceed);

			Finish();
		}
	}
}