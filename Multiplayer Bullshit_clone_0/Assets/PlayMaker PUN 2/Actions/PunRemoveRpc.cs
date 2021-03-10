// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Remove all buffered RPCs from server that were sent by targetPlayer. Can only be called on local player (for 'self') or Master Client (for anyone).")]
	public class PunRemoveBufferedRpc : FsmStateAction
	{
		[Tooltip("The Photon player")]
		[RequiredField]
		public PlayerReferenceProperty player;

		[Tooltip("Send this event if the action was executed")]
		public FsmEvent successEvent;
		
		[Tooltip("Send this event if the action was not executed, likely because we are not on the master and photonPlayer doesn't point to self or player id is wrong.")]
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
			if (!PhotonNetwork.InRoom)
			{
				LogError("Operation only allowed when inside a room");
				Fsm.Event(failureEvent);
				return;
			}
			
			_player = player.GetPlayer(this);
			
			if (_player != null)
			{
				if (!_player.Equals(PhotonNetwork.LocalPlayer) && !PhotonNetwork.IsMasterClient)
				{
					LogError("Removing a player's Buffered RPC different then your LocalPlayer is not allowed, unless you are the current MasterClient");
					Fsm.Event(failureEvent);
				}
				else
				{
					PhotonNetwork.RemoveRPCs(_player);
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
