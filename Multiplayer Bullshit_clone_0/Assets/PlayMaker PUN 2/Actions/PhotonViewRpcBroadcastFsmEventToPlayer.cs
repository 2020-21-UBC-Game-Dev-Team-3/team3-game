// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using UnityEngine;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Remote Event Calls (using Photon RPC under the hood) let you send a Fsm Event to a specific photon player.")]
	//[HelpUrl("")]
	public class PhotonViewRpcBroadcastFsmEventToPlayer : FsmStateAction
	{
		
		[RequiredField]
		[Tooltip("The targeted player")]
		public PlayerReferenceProperty player;
		
		[Tooltip("Leave to BroadCastAll.")]
		public FsmEventTarget eventTarget;
		
		[RequiredField]
		[Tooltip("The event you want to send.")]
		[UIHint(UIHint.FsmEvent)]
		public FsmEvent remoteEvent;
		
		[Tooltip("Optional string data ( will be injected in the Event data. Use 'get Event Info' action to retrieve it)")]
		public FsmString stringData;

		private Player _player;
		
		public override void Reset()
		{
			player = null;
	
			eventTarget = new FsmEventTarget();
			eventTarget.target = FsmEventTarget.EventTarget.BroadcastAll;
			remoteEvent = null;
			stringData = null;
		}

		public override void OnEnter()
		{
			ExecuteAction();
			
			Finish();
		}

		void ExecuteAction()
		{

			if (remoteEvent.Name ==""){
				return;
			}

			if (PlayMakerPhotonProxy.Instance==null)
			{
				Debug.LogError("PlayMakerPhotonProxy is missing in the scene");
				return;
			}
			

			_player = player.GetPlayer(this);

			if (_player == null)
			{
				return;
			}
			
			if (! stringData.IsNone && stringData.Value != ""){
				PlayMakerPhotonProxy.Instance.PhotonRpcFsmBroadcastEventWithString(_player,remoteEvent.Name,stringData.Value);
			}else{
				PlayMakerPhotonProxy.Instance.PhotonRpcBroadcastFsmEvent(_player,remoteEvent.Name);
			}	
		}

		public override string ErrorCheck()
		{
			if (eventTarget.target != FsmEventTarget.EventTarget.BroadcastAll)
			{
				return "eventTarget must be set to broadcast";	
			}
			
			if (remoteEvent == null)
			{
				return "Remote Event not set";
			}
	
			return string.Empty;
		}
	}
}