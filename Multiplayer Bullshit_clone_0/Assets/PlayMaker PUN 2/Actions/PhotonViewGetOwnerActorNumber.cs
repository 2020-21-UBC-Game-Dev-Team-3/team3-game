// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get the ActorNumbers of the owner for a given PhotonView .\n A PhotonView component is required on the gameObject")]
	[HelpUrl("")]
	public class PhotonViewGetOwnerActorNumber : PunComponentActionBase<PhotonView>
    {
		[RequiredField]
		[CheckForComponent(typeof(PhotonView))]
		[Tooltip("The Game Object with the PhotonView attached.")]
		public FsmOwnerDefault gameObject;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("The PhotonView Owner ActorNumber as int")]
		public FsmInt ownerActorNumber;

        [Tooltip("Send this event if there was no PhotonView found on the GamoObject")]
        public FsmEvent failure;

        public override void Reset()
		{
			gameObject = null;
            ownerActorNumber = null;
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

            if (!ownerActorNumber.IsNone)
            {
                ownerActorNumber.Value = this.photonView.OwnerActorNr;
            }
        }
	}
}