// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get the count of players currently inside a room. \n" +
		"This is updated on the MasterServer (only) in 5sec intervals (if any count changed).")]
	[HelpUrl("")]
	public class PhotonNetworkGetPlayersInRoomsCount : PunActionBase
	{
		[UIHint(UIHint.Variable)]
		[Tooltip("The number of players currently inside a room.")]
		[RequiredField]
		public FsmInt playersInRoomsCount;
		
		[Tooltip("Repeat every frame")]
		public bool everyFrame;
		
		public override void Reset()
		{
			playersInRoomsCount = null;
			everyFrame = false;
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
			playersInRoomsCount.Value = PhotonNetwork.CountOfPlayersInRooms;
			
		}
	}
}