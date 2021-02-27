// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

#pragma warning disable 168

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Can be used to return to a room after a disconnect and reconnect" +
		"After losing connection, you might be able to return to a room and continue playing," +
		"if the client is reconnecting fast enough. Use Reconnect() and this method." +
		"Cache the room name you're in and use ReJoin(roomname) to return to a game." +
		"" +
		"Note: To be able to ReJoin any room, you need to use UserIDs!" +
		"You also need to set RoomOptions.PlayerTtl." +
		"" +
		"Important: Instantiate() and use of RPCs is not yet supported." +
		"The ownership rules of PhotonViews prevent a seamless return to a game." +
		"Use Custom Properties and RaiseEvent with event caching instead." +
		"" +
		"Common use case: Press the Lock Button on a iOS device and you get disconnected immediately.")]
	[HelpUrl("")]
	public class PunRejoinRoom : PunActionBase
	{
		[Tooltip("The room Name")]
		public FsmString roomName;
		
		[ActionSection("Result")]
		[UIHint(UIHint.Variable)]
		[Tooltip("false if there was a problem, like roomname not found. True will attempt rejoin")]
		public FsmBool result;
		
		[Tooltip("Event to send if the reconnection will be attempted")]
		public FsmEvent willProceed;
		
		[Tooltip("Event to send if there was a problem, like roomname not found")]
		public FsmEvent willNotProceed;
		
		public override void Reset()
		{
			roomName  = null;
			result = null;
			willProceed = null;
			willNotProceed = null;
		}

		public override void OnEnter()
		{
			bool _result =  false;

			if (PhotonNetwork.IsConnectedAndReady && ! string.IsNullOrEmpty(roomName.Value))
			{
				_result = PhotonNetwork.RejoinRoom(roomName.Value);
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