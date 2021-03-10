// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Test if the Photon network is connected.")]
	[HelpUrl("")]
	public class PhotonNetworkGetIsConnected : PunActionBase
	{
		
		[UIHint(UIHint.Variable)]
		[Tooltip("True if the Photon network is connected.")]
		public FsmBool isConnected;
		
		[Tooltip("Send this event if the Photon network is connected.")]
		public FsmEvent isConnectedEvent;
		
		[Tooltip("Send this event if the Photon network is NOT connected.")]
		public FsmEvent isNotConnectedEvent;
		
		public override void Reset()
		{
			isConnected = null;
			isConnectedEvent = null;
			isNotConnectedEvent = null;
		}

		public override void OnEnter()
		{
			checkIsConnected();
			
			Finish();
		}
		
		void checkIsConnected()
		{
			bool _isConnected = PhotonNetwork.IsConnected;
			isConnected.Value = _isConnected;
			
			if (_isConnected )
			{
				if (isConnectedEvent!=null)
				{
					Fsm.Event(isConnectedEvent);
				}
			}
			else if (isNotConnectedEvent!=null)
			{
				Fsm.Event(isNotConnectedEvent);
			}
		}

	}
}