// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("This method wraps loading a level asynchronously and pausing network messages during the process.\n" +
	         "To sync the loaded level in a room, set AutomaticallySyncScene to true.\n" +
	         "Use PunGetLoadLevelProgress action to check on loading progress. Use Unity own scenes loading call backs to check for actual level loading completion")]
	[HelpUrl("")]
	public class PunLoadLevel : PunActionBase
	{
		[Tooltip("the Level Name")]
		public FsmString levelName;

		[Tooltip("Or the Level Number")]
		public FsmInt orLevelNumber;

		public override void Reset()
		{
			levelName = null;
			orLevelNumber = null;
		}

		public override void OnEnter()
		{
			ExecuteAction();
			
			Finish();
		}
		
		void ExecuteAction()
		{
			if (!levelName.IsNone)
			{
				PhotonNetwork.LoadLevel(levelName.Value);
			}
			else if (!orLevelNumber.IsNone)
			{
				PhotonNetwork.LoadLevel(orLevelNumber.Value);
			}
		}
	
	}
}