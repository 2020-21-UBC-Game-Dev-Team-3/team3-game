// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get how many times per second PhotonNetwork should send a package")]
	[HelpUrl("")]
	public class PunGetSendRate : FsmStateAction
	{

		[Tooltip("How many times per second PhotonNetwork should send a package")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt sendRate;

		[Tooltip("Execute every update")]
		public bool everyFrame;

		public override void Reset()
		{
			sendRate = null;
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			DoGetSendRate();

			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			DoGetSendRate();
		}
		
		void DoGetSendRate()
		{
			if (!sendRate.IsNone)	sendRate.Value = PhotonNetwork.SendRate;
		}
		
	}
}