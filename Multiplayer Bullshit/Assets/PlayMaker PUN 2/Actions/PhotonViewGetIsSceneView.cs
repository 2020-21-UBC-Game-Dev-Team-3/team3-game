// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("True if the PhotonView was loaded with the scene (game object) or instantiated with InstantiateSceneObject." +
		"\n Scene objects are not owned by a particular player but belong to the scene. " +
		"Thus they don't get destroyed when their creator leaves the game and the current Master Client can control them (whoever that is)." +
		" The ownerIs is 0 (player IDs are 1 and up). \n A PhotonView component is required on the gameObject")]
	[HelpUrl("")]
	public class PhotonViewGetIsSceneView : PunComponentActionBase<PhotonView>
    {
		[RequiredField]
		[CheckForComponent(typeof(PhotonView))]
		[Tooltip("The Game Object with the PhotonView attached.")]
		public FsmOwnerDefault gameObject;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("True if the Photon network view is a scene view.")]
		public FsmBool isSceneView;
		
		[Tooltip("Send this event if the Photon network view is a scene view")]
		public FsmEvent isSceneViewEvent;
		
		[Tooltip("Send this event if the Photon network view is NOT a scene view")]
		public FsmEvent isNotSceneViewEvent;

        [Tooltip("Send this event if there was no PhotonView found on the GamoObject")]
        public FsmEvent failure;

        public override void Reset()
		{
			gameObject = null;
			isSceneView = null;
			isSceneViewEvent = null;
			isNotSceneViewEvent = null;
            failure = null;
		}

		public override void OnEnter()
		{
			
			ExecuteAction();
			
			Finish();
		}
		
		void ExecuteAction()
		{
            if (!UpdateCache(Fsm.GetOwnerDefaultTarget(gameObject)))
            {
                if (failure != null) Fsm.Event(failure);
                return;
            }

            bool _isSceneView = this.photonView.IsSceneView;
			isSceneView.Value = _isSceneView;
			
			if (_isSceneView )
			{
				if (isSceneViewEvent!=null)
				{
					Fsm.Event(isSceneViewEvent);
				}
			}
			else if (isNotSceneViewEvent!=null)
			{
				Fsm.Event(isNotSceneViewEvent);
			}
		}

	}
}