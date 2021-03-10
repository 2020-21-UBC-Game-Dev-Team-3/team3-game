// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Set a new MasterClient. Can only be done on the current MasterClient.")]
	[HelpUrl("")]
	public class PunSetMasterClient : FsmStateAction
	{
		[Tooltip("The Photon player")]
		[RequiredField]
		public PlayerReferenceProperty player;

		[Tooltip("false if setting the master failed, true if request was executed")]
		[UIHint(UIHint.Variable)]
		public FsmBool result;

		[Tooltip("event sent if request was executed")]
		public FsmEvent successEvent;

		[Tooltip("event sent if request was not executed, likely because not on the master")]
		public FsmEvent errorEvent;

		private Player _player;
		private bool _result;
		
		public override void Reset()
		{
			player = new PlayerReferenceProperty();
			result = null;
			successEvent = null;
			errorEvent = null;
		}
		
		public override void OnEnter()
		{
			ExecuteAction();
			
			Finish();
		}
		
		void ExecuteAction()
		{
			_player = player.GetPlayer(this);
			_result = false;
			
			if (_player == null)
			{
				LogError("Player reference is null");
				
			} else {

				if (!PhotonNetwork.IsMasterClient)
				{
					LogError("Operation can only be done on the current MasterClient");
				}
				else
				{

					_result = PhotonNetwork.SetMasterClient(_player);
				}

				if (!_result)
				{
					LogError("Operation Failed");
				}
			}

			if (!result.IsNone) {
				result.Value = _result;
			}

			Fsm.Event (_result ? successEvent : errorEvent);
		}
	}
}