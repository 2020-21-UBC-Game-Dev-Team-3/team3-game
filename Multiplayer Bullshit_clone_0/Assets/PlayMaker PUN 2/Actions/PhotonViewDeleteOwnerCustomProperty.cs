// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Delete the owner custom property of a GameObject.\n A PhotonView component is required on the gameObject")]
	[HelpUrl("")]
	public class PhotonViewDeleteOwnerCustomProperty : PunComponentActionBase<PhotonView>
	{
		
		[RequiredField]
		[CheckForComponent(typeof(PhotonView))]
		[Tooltip("The Game Object with the PhotonView attached.")]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("The custom property key to delete")]
		public FsmString customPropertyKey;
		
		[Tooltip("Send this event if the custom property was deleted")]
		public FsmEvent success;
		
		[Tooltip("Send this event if the custom property deletion failed")]
		public FsmEvent failure;
		
		
		public override void Reset()
		{
			gameObject = null;
			customPropertyKey = "My Property";
			success = null;
			failure = null;
		}
		
		public override void OnEnter()
		{
			if (!UpdateCache(Fsm.GetOwnerDefaultTarget(gameObject)))
			{
				if (failure != null) Fsm.Event(failure);
				return;
			}
			
			if (DeleteOwnerProperty())
			{
				Fsm.Event(success);	
			}else{
				Fsm.Event(failure);	
			}
			
			Finish();
		}
		
		private bool DeleteOwnerProperty()
		{
			Player _player = this.photonView.Owner;
			if (_player==null)
			{
				return false;
			}
			
			ExitGames.Client.Photon.Hashtable _prop = new ExitGames.Client.Photon.Hashtable();
			
			_prop[customPropertyKey.Value] = null;
			
			_player.SetCustomProperties(_prop);
			
			return true;
		}
		
	}
}