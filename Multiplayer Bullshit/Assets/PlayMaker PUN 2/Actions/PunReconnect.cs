// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Can be used to reconnect to the master server after a disconnect. After losing connection, you can use this to connect a client to the region Master Server again." +
		"Cache the room name you're in and use ReJoin(roomname) to return to a game." +
		"Common use case: Press the Lock Button on a iOS device and you get disconnected immediately.")]
	[HelpUrl("")]
	public class PhotonNetworkReconnect : PunActionBase
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

			bool _result = PhotonNetwork.Reconnect();
			if (!result.IsNone)
			{
				result.Value = _result;
			}

			Fsm.Event(_result ? willProceed : willNotProceed);

			Finish();
		}
	}
}