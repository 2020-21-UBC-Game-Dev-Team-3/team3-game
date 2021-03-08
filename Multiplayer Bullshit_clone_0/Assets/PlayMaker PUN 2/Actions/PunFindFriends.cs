// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Requests the rooms and online status for a list of friends. Watch for PHOTON / FRIEND LIST UPDATED to start getting friends data")]
	[HelpUrl("")]
	public class PunFindFriends : PunActionBase
	{
		[RequiredField]
		[Tooltip("The list of friends ")]
		[UIHint(UIHint.Variable)]
		[ArrayEditor(VariableType.String)]
		public FsmArray friendlist;

        [ActionSection("Result")]
        [UIHint(UIHint.Variable)]
        [Tooltip("false if the friendlist request will not be attempted. True will attempt request")]
        public FsmBool result;

        [Tooltip("Event to send if the friendlist request will be attempted")]
        public FsmEvent willProceed;

        [Tooltip("Event to send if the friendlist request will not be attempted")]
        public FsmEvent willNotProceed;

        public override void Reset()
		{
			friendlist = null;
            result = null;
            willProceed = null;
            willNotProceed = null;
        }
		
		public override void OnEnter()
		{
            bool _result = PhotonNetwork.FindFriends(friendlist.stringValues);

            if (!result.IsNone)
            {
                result.Value = _result;
            }

            Fsm.Event(_result ? willProceed : willNotProceed);

            Finish();
		}
	}
}