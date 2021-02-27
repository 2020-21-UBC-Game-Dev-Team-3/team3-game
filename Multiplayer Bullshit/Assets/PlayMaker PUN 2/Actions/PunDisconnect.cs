// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Disconnect to Photon Network: \n" +
		"Makes this client disconnect from the photon server, a process that leaves any room and calls OnDisconnectedFromPhoton on completion.")]
	[HelpUrl("")]
	public class PunDisconnect : PunActionBase
	{
		public override void OnEnter()
		{
			PhotonNetwork.Disconnect();
			Finish();
		}
	}
}