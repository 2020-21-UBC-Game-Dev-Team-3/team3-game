// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Receives Network Ping and Stores in a String or an Int")]
	public class PhotonGetPing : PunActionBase
	{
		[Tooltip("The current roundtrip time to the photon server")]
		[UIHint(UIHint.Variable)]
		public FsmInt ping;

		[Tooltip("The current roundtrip time to the photon server as string")]
		[UIHint(UIHint.Variable)]
		public FsmString pingString;

		[Tooltip("Execute every update")]
		public bool everyFrame;

		public override void Reset()
		{
			ping = null;
			pingString = null;
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			DoGetPing();

			if (!everyFrame)
			{
				Finish();
			}
		}
		public override void OnUpdate()
		{
			DoGetPing();
		}
		
		void DoGetPing()
		{
			if (!ping.IsNone)	ping.Value = PhotonNetwork.GetPing ();
			if (!pingString.IsNone) pingString.Value = ping.Value.ToString();
		}
	}
}