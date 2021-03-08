// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Test if the Photon network View is controlled by a GameObject. \n A PhotonView component is required on the gameObject")]
	[HelpUrl("")]
	public class PhotonViewGetIsMine : PunComponentActionBase<PhotonView>
    {
		[CheckForComponent(typeof(PhotonView))]
		[Tooltip("The Game Object with the PhotonView attached.")]
		public FsmOwnerDefault gameObject;

	    public FsmBool searchInParent;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("True if the Photon network view is controlled by this object.")]
		public FsmBool isMine;
		
	    
	    [UIHint(UIHint.Variable)]
	    [Tooltip("True if the Photon network view is not controlled by this object.")]
	    public FsmBool isNotMine;
	    
		[Tooltip("Send this event if the Photon network view controlled by this object.")]
		public FsmEvent isMineEvent;
		
		[Tooltip("Send this event if the Photon network view is NOT controlled by this object.")]
		public FsmEvent isNotMineEvent;

        [Tooltip("Send this event if there was no PhotonView found on the GamoObject")]
        public FsmEvent failure;

		
		public override void Reset()
		{
			gameObject = null;
			searchInParent = true;
			isMine = null;
			isNotMine = null;
			isMineEvent = null;
			isNotMineEvent = null;
            failure = null;
        }

		public override void OnEnter()
		{
            ExecuteAction();
			
			Finish();
		}
		
		void ExecuteAction()
		{
			if (!UpdateCache(Fsm.GetOwnerDefaultTarget(gameObject),searchParent: searchInParent.Value))
			{
                if (failure != null) Fsm.Event(failure);
				return;	
			}

			bool _isMine = this.photonView.IsMine;
			if (!isMine.IsNone) isMine.Value = _isMine;
			
			if (!isNotMine.IsNone) isNotMine.Value = !_isMine;
			
			if (_isMine )
			{
				if (isMineEvent!=null)
				{
					Fsm.Event(isMineEvent);
				}
			}
			else if (isNotMineEvent!=null)
			{
				Fsm.Event(isNotMineEvent);
			}
		}

	}
}