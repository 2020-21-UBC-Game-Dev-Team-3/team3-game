// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Enable or disable the processing of Photon network messages.\n\n" +
	         "While IsMessageQueueRunning == false, the OnPhotonSerializeView calls are not done and nothing is sent by " +
	         "a client. Also, incoming messages will be queued until you re-activate the message queue.\n" +
	         "This can be useful if you first want to load a level, then go on receiving data of PhotonViews and RPCs.\n" +
	         "The client will go on receiving and sending acknowledgements for incoming packages and your RPCs/Events.\n" +
	         "This adds 'lag' and can cause issues when the pause is longer, as all incoming messages are just queued.")]
	[HelpUrl("")]
	public class PunSetIsMessageQueueRunning : FsmStateAction
	{
		[Tooltip("Is Message Queue Running. If this is disabled no Photon RPC call execution or Photon network view synchronization takes place")]
		public FsmBool isMessageQueueRunning;
		
		public override void Reset()
		{
			isMessageQueueRunning = null;
		}

		public override void OnEnter()
		{
			PhotonNetwork.IsMessageQueueRunning = isMessageQueueRunning.Value;
			
			Finish();
		}

	}
}