// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get how many times per second OnPhotonSerialize should be called on PhotonViews")]
	[HelpUrl("")]
	public class PunGetSerializationRate : FsmStateAction
	{

		[Tooltip("how many times per second OnPhotonSerialize should be called on PhotonViews")]
		[UIHint(UIHint.Variable)]
		public FsmInt serializationRate;

		[Tooltip("Execute every update")]
		public bool everyFrame;

		public override void Reset()
		{
			serializationRate = null;
			everyFrame = false;
		}
		
		public override void OnEnter()
		{
			DoGetSendRateOnSerialize();

			if (!everyFrame)
			{
				Finish();
			}
		}
		public override void OnUpdate()
		{
			DoGetSendRateOnSerialize();
		}
		
		void DoGetSendRateOnSerialize()
		{
			if (!serializationRate.IsNone) serializationRate.Value = PhotonNetwork.SerializationRate;
		}
		
	}
}