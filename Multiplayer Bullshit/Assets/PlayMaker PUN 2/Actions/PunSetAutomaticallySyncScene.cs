// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Defines if all clients in a room should automatically load the same level as the Master Client.\n" +
		"When enabled, clients load the same scene that is active on the Master Client.\n" +
		"When a client joins a room, the scene gets loaded even before the callback OnJoinedRoom gets called.\n" +
		"To synchronize the loaded level, the Master Client should use PhotonNetwork.LoadLevel, which notifies the other clients before starting to load the scene.\n" +
		"If the Master Client loads a level directly via Unity's API, PUN will notify the other players after the scene loading completed (using SceneManager.sceneLoaded).")]
	public class PunSetAutomaticallySyncScene : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Defines how many times per second PhotonNetwork should send a package, default is 20")]
		public FsmBool AutomaticallySyncScene;

		public override void Reset()
		{
			AutomaticallySyncScene = false;
		}
		
		public override void OnEnter()
		{
			ExecuteAction();
			
			Finish();
		}
		
		void ExecuteAction()
		{
			if(!AutomaticallySyncScene.IsNone)
			{
				PhotonNetwork.AutomaticallySyncScene = AutomaticallySyncScene.Value;
			}else
			{
				LogError("AutomaticallySyncScene is undefined");
			}
		}
	}
}
