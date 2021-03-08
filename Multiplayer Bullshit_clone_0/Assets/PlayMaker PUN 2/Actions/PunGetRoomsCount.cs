// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get the number of rooms available.Make sure you call PunGetRoomList first. Use this action after youy received PUN / ON ROOM LIST UPDATE")]
	[HelpUrl("")]
	public class PunGetRoomsCount : PunActionBase
	{
		
		[UIHint(UIHint.Variable)]
        [RequiredField]
		[Tooltip("The number of available rooms.")]
		public FsmInt roomsCount;
		
		[Tooltip("Repeat every frame")]
		public bool everyFrame;
		
		public override void Reset()
		{
            roomsCount = null;
		}

		public override void OnEnter()
		{
			ExecuteAction();
			
		    if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
            ExecuteAction();
		}
		
		void ExecuteAction()
		{
            roomsCount.Value = PlayMakerPunCallbacksProxy.Instance.RoomListCount;
        }
	}
}