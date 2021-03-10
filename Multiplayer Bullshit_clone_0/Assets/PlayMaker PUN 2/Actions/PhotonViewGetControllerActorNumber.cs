// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get the ActorNumber of the controller for a given PhotonView .\n A PhotonView component is required on the gameObject")]
	[HelpUrl("")]
	public class PhotonViewGetcontrollerActorNumbers : PunComponentActionBase<PhotonView>
    {
		[RequiredField]
		[CheckForComponent(typeof(PhotonView))]
		[Tooltip("The Game Object with the PhotonView attached.")]
		public FsmOwnerDefault gameObject;
		
        [UIHint(UIHint.Variable)]
        [Tooltip("The PhotonView controller ActorNumber")]
        public FsmInt controllerActorNumber;

        [Tooltip("Send this event if there was no PhotonView found on the GamoObject")]
        public FsmEvent failure;

        public override void Reset()
		{
			gameObject = null;
            controllerActorNumber = null;
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

            if (!controllerActorNumber.IsNone)
            {
                controllerActorNumber.Value = this.photonView.ControllerActorNr;
            }
        }

	}
}