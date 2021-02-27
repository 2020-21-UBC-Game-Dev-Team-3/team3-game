// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Updates and synchronizes the named custom property of this Room. New properties are added, existing values are updated.")]
	[HelpUrl("")]
	public class PhotonNetworkSetRoomCustomProperty : PunActionBase
	{
		[Tooltip("The Custom Property to set or update")]
		public FsmString customPropertyKey;
		
		[RequiredField]
		[Tooltip("Value of the property")]
		public FsmVar customPropertyValue;
		
		[Tooltip("Send this event if the custom property was set or update.")]
		public FsmEvent successEvent;
		
		[Tooltip("Send this event if the custom property set failed, likely because we are not in a room.")]
		public FsmEvent failureEvent;

		public override void Reset()
		{
			customPropertyKey = "My Property";
			customPropertyValue = null;
			successEvent = null;
			failureEvent = null;
		}
		
		public override void OnEnter()
		{
			SetRoomProperty();
			
			Finish();
		}
		
		void SetRoomProperty()
		{

			if (!PhotonNetwork.InRoom )
			{
				Fsm.Event(failureEvent);	
				return;
			}
			
			ExitGames.Client.Photon.Hashtable _prop = new ExitGames.Client.Photon.Hashtable();
			
			_prop[customPropertyKey.Value] =  PlayMakerUtils.GetValueFromFsmVar(this.Fsm,customPropertyValue);
			
			PhotonNetwork.CurrentRoom.SetCustomProperties(_prop);
			
			Fsm.Event(successEvent);
		}

	}
}