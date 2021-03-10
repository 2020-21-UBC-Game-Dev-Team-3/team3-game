// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Realtime;
using UnityEngine;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Retrieve the player properties of the last photon message (OnPhotonPlayerConnected, OnPhotonPlayerDisconnected, OnPhotonPlayerPropertiesChanged or OnMasterClientSwitched).")]
	[HelpUrl("")]
	public class PunGetLastMessagePlayerProperties : FsmStateAction
	{
		
		[ActionSection("Player Properties")] 
		
		[Tooltip("The Photon player nickName")]
		[UIHint(UIHint.Variable)]
		public FsmString nickName;
		
		[Tooltip("The Photon player USerId")]
		[UIHint(UIHint.Variable)]
		public FsmString userId;
		
		[Tooltip("The Photon player ActorNumber")]
		[UIHint(UIHint.Variable)]
		public FsmInt actorNumber;
		
		[Tooltip("The Photon player isLocal property")]
		[UIHint(UIHint.Variable)]
		public FsmBool isLocal;
		
		[Tooltip("The Photon player isLocal isMasterClient")]
		[UIHint(UIHint.Variable)]
		public FsmBool isMasterClient;

		
		[Tooltip("Custom Properties you have assigned to this player.")]
		[CompoundArray("player Custom Properties", "property", "value")]
		public FsmString[] customPropertyKeys;
		[UIHint(UIHint.Variable)]
		public FsmVar[] customPropertiesValues;
		
		[ActionSection("Events")] 
		
		[Tooltip("Send this event if the player was found.")]
		public FsmEvent successEvent;
		
		[Tooltip("Send this event if no player was found.")]
		public FsmEvent failureEvent;
			
		
		public override void Reset()
		{
			nickName = null;
			userId = null;
			
			actorNumber = null;
			isLocal = null;
			isMasterClient = null;
			successEvent = null;
			failureEvent = null;
			
			customPropertyKeys = new FsmString[0];
			customPropertiesValues = new FsmVar[0];
		}

		public override void OnEnter()
		{
			bool ok = getLastMessagePlayerProperties();

			Fsm.Event(ok ? successEvent : failureEvent);
			
			Finish();
		}

		bool getLastMessagePlayerProperties()
		{
			if (PlayMakerPhotonProxy.Instance==null)
			{
				Debug.LogError("PlayMakerPhotonProxy is missing in the scene");
				return false;
			}
			
			Player _player = PlayMakerPhotonProxy.Instance.lastMessagePhotonPlayer;
			if (_player==null)
			{
				return false;
			}
			
			nickName.Value = _player.NickName;
			actorNumber.Value   = _player.ActorNumber;
			userId.Value = _player.UserId;
			isLocal.Value = _player.IsLocal;
			isMasterClient.Value = _player.IsMasterClient;
			
			// get the custom properties
			int i = 0;
			foreach(FsmString key in customPropertyKeys)
			{
				if (!_player.CustomProperties.ContainsKey(key.Value))
				{
					return false;
				}
				PlayMakerUtils.ApplyValueToFsmVar(this.Fsm,customPropertiesValues[i],_player.CustomProperties[key.Value]);
				i++;
			}
			
			return true;
		}
	
		
		
	}
}