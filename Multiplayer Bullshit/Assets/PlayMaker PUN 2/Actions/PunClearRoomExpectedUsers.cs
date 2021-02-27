// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Attempts to remove all current expected users from the server's Slot Reservation list.")]
	[HelpUrl("")]
	public class PunClearroomExpectedUsers : FsmStateAction
	{
		[Tooltip("Send this event if the action was executed")]
		public FsmEvent successEvent;
		
		[Tooltip("Send this event if the action was not executed, likely because we are not in a room.")]
		public FsmEvent failureEvent;

		public override void OnEnter()
		{
			if (PhotonNetwork.CurrentRoom == null)
			{
				Fsm.Event(failureEvent);
			}
			else
			{
				PhotonNetwork.CurrentRoom.ClearExpectedUsers();
			}

			Fsm.Event(successEvent);

			Finish();
		}
	}
}