// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("If true, Switch to alternative ports for a UDP connection to the Public Cloud.\n" +
	         "This should be used when a customer has issues with connection stability. Some players" +
	         "reported better connectivity for Steam games. The effect might vary, which is why the" +
	         "alternative ports are not the new default.")]
	[HelpUrl("")]
	public class PunSetUseAlternativeUdpPorts : FsmStateAction
	{
		[Tooltip("If true, uses alternative ports for a UDP connection to the Public Cloud")]
		public FsmBool useAlternativeUdpPorts;
		
		public override void Reset()
		{
			useAlternativeUdpPorts  = null;
		}

		public override void OnEnter()
		{
			PhotonNetwork.UseAlternativeUdpPorts = useAlternativeUdpPorts.Value;
			
			Finish();
		}
	}
}