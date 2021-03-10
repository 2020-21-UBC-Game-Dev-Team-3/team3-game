// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Gets Photon networking client connection cloud region")]
    [HelpUrl("")]
	public class PunGetCloudRegion : PunActionBase
	{

        [Tooltip("The cloud region currently connected to")]
        [UIHint(UIHint.Variable)]
        public FsmString cloudRegion;
 
		[Tooltip("Repeat every frame. Useful for watching the property over time.")]
		public bool everyFrame;

		public override void Reset()
		{
            cloudRegion = null;
            everyFrame = false;
		}

		public override void OnEnter()
		{
            ExecuteAction();

            if (!everyFrame)
			{
                Finish();
			}
        }

        public override void OnUpdate()
        {
            ExecuteAction();
        }

        void ExecuteAction()
        {
            cloudRegion.Value = PhotonNetwork.CloudRegion;
        }

	}
}