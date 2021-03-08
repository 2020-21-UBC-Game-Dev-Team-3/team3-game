// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Gets the Photon networking app version")]
    [HelpUrl("")]
	public class PunGetAppVersion : PunActionBase
	{

        [Tooltip("The app Version")]
        [UIHint(UIHint.Variable)]
        public FsmString appVersion;
 
		[Tooltip("Repeat every frame. Useful for watching the property over time.")]
		public bool everyFrame;

		public override void Reset()
		{
			appVersion = null;
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
            appVersion.Value = PhotonNetwork.AppVersion;
        }

	}
}