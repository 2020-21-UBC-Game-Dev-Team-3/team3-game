// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get the scene loading progress when using LoadLevel()./n" +
	         "The value is 0 if the app never loaded a scene with LoadLevel().During async scene loading, the value is between 0 and 1./n" +
	         "Once any scene completed loading, it stays at 1 (signaling 'done').")]
	[HelpUrl("")]
	public class PunGetLevelLoadingProgress : FsmStateAction
	{
		[UIHint(UIHint.Variable)]
        [RequiredField]
		[Tooltip("The scene loading progress. Ranged from 0 to 1")]
		public FsmFloat levelLoadingProgress;
		
		[Tooltip("Repeat every frame")]
		public bool everyFrame;
		
		public override void Reset()
		{
			levelLoadingProgress = null;
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
			levelLoadingProgress.Value = PhotonNetwork.LevelLoadingProgress;
		}

	}
}