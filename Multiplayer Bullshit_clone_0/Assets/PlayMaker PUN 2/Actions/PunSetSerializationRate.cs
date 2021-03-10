// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using UnityEngine;
using Photon.Pun;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Defines how many times per second OnPhotonSerialize should be called on PhotonViews. Default is 10.\n" +
		"Choose this value in relation to PhotonNetwork.sendRate. OnPhotonSerialize will create updates and messages to be sent.\n" +
		"A lower rate takes up less performance but will cause more lag.")]
	public class PunSetSerializationRate : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Defines how many times per second OnPhotonSerialize should be called on PhotonViews, default is 10")]
		public FsmInt serializationRate;
		
		public override void Reset()
		{
			serializationRate = 10;
		}
		
		public override void OnEnter()
		{
			DoSetSendRateOnSerialize();
			
			Finish();
		}
		
		void DoSetSendRateOnSerialize()
		{
			if (!serializationRate.IsNone)
			{
				PhotonNetwork.SerializationRate = serializationRate.Value;
			}else
			{
				LogError("serializationRate is undefined");
			}
		}
	}
}
