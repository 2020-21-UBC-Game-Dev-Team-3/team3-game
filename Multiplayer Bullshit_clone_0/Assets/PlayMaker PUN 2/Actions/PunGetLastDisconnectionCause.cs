// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Realtime;
using UnityEngine;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Retrieve the the last photon disconnection cause")]
	[HelpUrl("")]
	public class PunGetLastDisconnectionCause : FsmStateAction
	{

		[UIHint(UIHint.Variable)]
		[Tooltip("The last disconnection Cause")]
		[ObjectType(typeof(DisconnectCause))]
		public FsmEnum lastDisconnectionCause;
		
		
		[Tooltip("The last disconnection Cause as string")]
		[UIHint(UIHint.Variable)]
		public FsmString lastDisconnectionCauseAsString;
		
		
		
		[ActionSection("Events")] 
		
		[Tooltip("Send this event if the pun proxy was found.")]
		public FsmEvent successEvent;
		
		[Tooltip("Send this event if pun proxy was not found.")]
		public FsmEvent failureEvent;
		
		public override void Reset()
		{
			lastDisconnectionCause = null;
			
			lastDisconnectionCauseAsString = null;
		}

		public override void OnEnter()
		{
			bool ok = GetData();

			Fsm.Event(ok ? successEvent : failureEvent);

			Finish();
		}

		bool GetData()
		{
			if (PlayMakerPunCallbacksProxy.Instance==null)
			{
				Debug.LogError("PlayMakerPunCallbacksProxy is missing in the scene");
				return false;
			}

			if (!lastDisconnectionCauseAsString.IsNone)
			{
				lastDisconnectionCauseAsString.Value = PlayMakerPunCallbacksProxy.Instance.lastDisconnectCause.ToString();
			}

			if (!lastDisconnectionCause.IsNone)
			{
				lastDisconnectionCause.Value = PlayMakerPunCallbacksProxy.Instance.lastDisconnectCause;
			}

			return true;
		}
	
		
		
	}
}