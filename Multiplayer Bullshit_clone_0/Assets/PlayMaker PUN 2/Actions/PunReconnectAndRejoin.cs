// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("When the client lost connection during gameplay, this action attempts to reconnect and rejoin the room." +
		"This method re-connects directly to the game server which was hosting the room PUN was in before." +
		"If the room was shut down in the meantime, PUN will call OnPhotonJoinRoomFailed and return this client to the Master Server." +
		"" +
		"Check the return value, if this client will attempt a reconnect and rejoin (if the conditions are met)." +
		"If ReconnectAndRejoin returns false, you can still attempt a Reconnect and ReJoin." +
		"" +
		"Similar to PhotonNetwork.ReJoin, this requires you to use unique IDs per player (the UserID).")]
	[HelpUrl("")]
	public class PunReconnectAndRejoin : PunActionBase
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
			
			bool _result = PhotonNetwork.ReconnectAndRejoin();
			if (!result.IsNone)
			{
				result.Value = _result;
			}
			
			Fsm.Event(_result ? willProceed : willNotProceed);
			
			Finish();
		}
	}
}