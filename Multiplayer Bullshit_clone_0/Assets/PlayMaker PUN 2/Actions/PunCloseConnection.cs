// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Request a client to disconnect (KICK). Only the master client can do this" +
		"Only the target player gets this event. That player will disconnect automatically, which is what the others will notice, too")]
	[HelpUrl("")]
	public class PunCloseConnection : PunActionBase
	{
		[Tooltip("The Photon player")]
		[RequiredField]
		public PlayerReferenceProperty player;

		[ActionSection("Result")]
		[UIHint(UIHint.Variable)]
		[Tooltip("false if there is no known room or game server to return to. True will attempt reconnection")]
		public FsmBool result;
		
		[Tooltip("Event to send if the reconnection will be attempted")]
		public FsmEvent willProceed;
		
		[Tooltip("Event to send if there is no known room or game server to return to")]
		public FsmEvent willNotProceed;

		public override void Reset()
		{
            player = new PlayerReferenceProperty();
			result = null;
			willProceed = null;
			willNotProceed = null;
		}

        bool HideNameIf()
        {
            return player.reference != PlayerReferenceProperty.PlayerReferences.ByNickName;
        }

        public override void OnEnter()
		{
           

            bool _result = PhotonNetwork.CloseConnection(player.GetPlayer(this));
			
            if (!result.IsNone)
            {
				result.Value = _result;
			}

			Fsm.Event(_result ? willProceed : willNotProceed);
			Finish();
		}
	}
}