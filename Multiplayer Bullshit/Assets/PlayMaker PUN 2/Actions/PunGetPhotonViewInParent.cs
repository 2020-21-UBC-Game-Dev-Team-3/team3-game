// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get a Photon network View on a GameObject or its parent.")]
	[HelpUrl("")]
	public class PunGetPhotonViewInParent : PunComponentActionBase<PhotonView>
    {
		[CheckForComponent(typeof(PhotonView))]
		[Tooltip("The Game Object with the PhotonView attached or one of its parent.")]
		public FsmOwnerDefault gameObject;

	    [Tooltip("True is a Photon network view is found on object or parents")]
	    public FsmBool success;
	    
	    [Tooltip("Send this event if a Photon network view found on object or parents")]
	    public FsmEvent found;
	    
		[Tooltip("Send this event if a Photon network view not found on object or parents")]
		public FsmEvent notFound;

		
		public override void Reset()
		{
			gameObject = null;
			success = null;
			found = null;
			notFound = null;
		}

		public override void OnEnter()
		{
            ExecuteAction();
			
			Finish();
		}
		
		void ExecuteAction()
		{
			if (!UpdateCache(Fsm.GetOwnerDefaultTarget(gameObject),searchParent: true))
			{
				success.Value = false;
                if (notFound != null) Fsm.Event(notFound);
				return;	
			}

			success.Value = true;
			if (found != null) Fsm.Event(found);
		}

	}
}