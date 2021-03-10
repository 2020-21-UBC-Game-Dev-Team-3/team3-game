// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Delete the named custom property of this Room.")]
	[HelpUrl("")]
	public class PunDeleteRoomCustomProperty : PunActionBase
	{
		
		[Tooltip("The custom property key to delete")]
		public FsmString customPropertyKey;
		
		[Tooltip("Send this event if the custom property was deleted")]
		public FsmEvent successEvent;
		
		[Tooltip("Send this event if the custom property deletion failed, likely because we are not in a room.")]
		public FsmEvent failureEvent;

		public override void Reset()
		{
			customPropertyKey = "My Property";
			successEvent = null;
			failureEvent = null;
		}
		
		public override void OnEnter()
		{
			DeleteRoomProperty();
			
			Finish();
		}
		
		void DeleteRoomProperty()
		{
			Room _room = PhotonNetwork.CurrentRoom;
			bool _isInRoom = _room!=null;
			
			if (!_isInRoom )
			{
				Fsm.Event(failureEvent);	
				return;
			}
			
			ExitGames.Client.Photon.Hashtable _prop = new ExitGames.Client.Photon.Hashtable();
			
			_prop[customPropertyKey.Value] = null;
			_room.SetCustomProperties(_prop);
			
			Fsm.Event(successEvent);
		}

	}
}