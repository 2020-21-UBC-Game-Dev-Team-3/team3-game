// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Updates and synchronizes the named custom property of a player. New properties are added, existing values are updated. CustomProperties can be set before entering a room and will be synced as well.")]
	[HelpUrl("")]
	public class PunSetPlayerCustomProperty : PunActionBase
	{
		
		[Tooltip("The Photon player")]
		[RequiredField]
		public PlayerReferenceProperty player;
		
		[Tooltip("The Custom Property to set or update")]
		public FsmString customPropertyKey;
		
		[RequiredField]
		[Tooltip("Value of the property")]
		public FsmVar customPropertyValue;

		[Tooltip("Send this event if the action was executed")]
		public FsmEvent successEvent;
		
		[Tooltip("Send this event if the action was not executed, likely because you are trying to destroy a player different from your localplayer and you are not the masterClient")]
		public FsmEvent failureEvent;
		
		private Player _player;
		
		public override void Reset()
		{
			player = new PlayerReferenceProperty();
			customPropertyKey = "My Property";
			customPropertyValue = null;
			successEvent = null;
			failureEvent = null;
		}
		
		public override void OnEnter()
		{
			SetPlayerProperty();
			
			Finish();
		}
		
		void SetPlayerProperty()
		{
			if (customPropertyValue==null)
			{
				LogError("customPropertyValue is null ");
				Fsm.Event(failureEvent);
				return;
			}
			
			_player = player.GetPlayer(this);
			
			if (_player!=null)
			{
				if (!_player.Equals(PhotonNetwork.LocalPlayer) && !PhotonNetwork.IsMasterClient)
				{
					LogError("setting a player custom property different from your LocalPlayer is not allowed, unless you are the current MasterClient");
					Fsm.Event(failureEvent);
				}
				else
				{
					ExitGames.Client.Photon.Hashtable _prop = new ExitGames.Client.Photon.Hashtable();
					//Log(" set key "+customPropertyKey.Value+"="+ PlayMakerUtils.GetValueFromFsmVar(this.Fsm,customPropertyValue));
			
					_prop[customPropertyKey.Value] = PlayMakerUtils.GetValueFromFsmVar(this.Fsm,customPropertyValue);
					_player.SetCustomProperties(_prop);
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