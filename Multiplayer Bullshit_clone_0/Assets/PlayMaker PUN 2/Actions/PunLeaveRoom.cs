// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using UnityEngine;
using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Leave the current game room. Optionnally can become inactive as opposed to gone")]
	[HelpUrl("")]
	public class PunLeaveRoom : PunActionBase
	{
        [Tooltip("If true, the player becomes inactive in the room he was in as opposed to gone")]
        public FsmBool becomeInactive;

        [ActionSection("Result")]
        [UIHint(UIHint.Variable)]
        [Tooltip("false if the room creation will not be attempted. True if it will attempt room creation")]
        public FsmBool result;

        [Tooltip("Event to send if the room creation will be attempted")]
        public FsmEvent willProceed;

        [Tooltip("Event to send if the room creation will not be attempted")]
        public FsmEvent willNotProceed;

        public override void Reset()
        {
            result = null;
            willProceed = null;
            willNotProceed = null;
        }

        public override void OnEnter()
		{
			bool _result = PhotonNetwork.LeaveRoom(becomeInactive.Value);

            if (!result.IsNone)
            {
                result.Value = _result;
            }

            Fsm.Event(_result ? willProceed : willNotProceed);


            Finish();
		}
	}
}