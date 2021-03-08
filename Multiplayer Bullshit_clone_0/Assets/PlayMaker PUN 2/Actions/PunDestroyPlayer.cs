// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Destroy all GameObjects/PhotonViews for a given player. Can only be called on the local player. The only exception is the master client which can call this for all players.")]
	[HelpUrl("")]
	public class PhotonNetworkDestroyPlayer : PunActionBase
	{
		[Tooltip("The Photon player")]
		[RequiredField]
		public PlayerReferenceProperty player;

		[Tooltip("Send this event if the action was executed")]
		public FsmEvent successEvent;
		
		[Tooltip("Send this event if the action was not executed, likely because you are trying to destroy a player different from your localplayer and you are not the masterClient")]
		public FsmEvent failureEvent;

		private Player _player;
		
		public override void Reset()
		{
			player = new PlayerReferenceProperty();
			successEvent = null;
			failureEvent = null;
		}

		public override void OnEnter()
		{
			ExecuteAction();
			
			Finish();
		}
		
		
		void ExecuteAction()
		{
			_player = player.GetPlayer(this);
			
			if (_player!=null)
			{
				if (!_player.Equals(PhotonNetwork.LocalPlayer) && !PhotonNetwork.IsMasterClient)
				{
					LogError("Destroying a player different from your LocalPlayer is not allowed, unless you are the current MasterClient");
					Fsm.Event(failureEvent);
				}
				else
				{
					PhotonNetwork.DestroyPlayerObjects(_player);
					Fsm.Event(successEvent);
				}
			}
			else
			{
				LogError("Player reference is null");
				Fsm.Event(failureEvent);
			}
			
		}
	}
}