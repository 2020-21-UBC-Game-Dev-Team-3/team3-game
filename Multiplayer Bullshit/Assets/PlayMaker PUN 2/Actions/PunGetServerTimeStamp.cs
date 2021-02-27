// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get The current server's millisecond timestamp." +
		"This can be useful to sync actions and events on all clients in one room. The timestamp is based on the server's Environment.TickCount." +
		"It will overflow from a positive to a negative value every so often, so be careful to use only time-differences to check the time delta when things happen." +
	    "This is the basis for PhotonNetwork.Time.")]
	[HelpUrl("")]
	public class PhotonNetworkGetServerTimeStamp : FsmStateAction
	{
		[Tooltip("The current server's millisecond timestamp")]
		[UIHint(UIHint.Variable)]
		public FsmFloat serverTimeStamp;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			serverTimeStamp = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			ExecuteAction();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			ExecuteAction();
		}

		void ExecuteAction()
		{
			serverTimeStamp.Value = (int)PhotonNetwork.ServerTimestamp;
		}
	}
}