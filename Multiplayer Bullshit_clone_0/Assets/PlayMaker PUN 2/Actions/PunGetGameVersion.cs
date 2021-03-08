// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Gets the Photon networking game version")]
    [HelpUrl("")]
	public class PunGetGameVersion : PunActionBase
	{

        [Tooltip("The game Version")]
        [UIHint(UIHint.Variable)]
        public FsmString gameVersion;
 
		[Tooltip("Repeat every frame. Useful for watching the property over time.")]
		public bool everyFrame;

		public override void Reset()
		{
			gameVersion = null;
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
            gameVersion.Value = PhotonNetwork.GameVersion;
        }

	}
}