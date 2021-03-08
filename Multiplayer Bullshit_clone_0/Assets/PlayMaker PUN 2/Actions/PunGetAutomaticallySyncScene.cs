// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Test if the AutomaticallySyncScene flag is true or not. It Defines if all clients in a room should automatically load the same level as the Master Client.")]
	[HelpUrl("")]
	public class PunGetAutomaticallySyncScene : PunActionBase
	{
		
		[UIHint(UIHint.Variable)]
		[Tooltip("The AutomaticallySyncScene value")]
		public FsmBool automaticallySyncScene;
		
		[Tooltip("Send this event if AutomaticallySyncScene is true.")]
		public FsmEvent isAutomaticallySyncSceneEvent;
		
		[Tooltip("Send this event if AutomaticallySyncScene is false.")]
		public FsmEvent isNotAutomaticallySyncSceneEvent;
        
        public override void Reset()
		{
            automaticallySyncScene = null;
            isAutomaticallySyncSceneEvent = null;
            isNotAutomaticallySyncSceneEvent = null;
        }

		public override void OnEnter()
		{
			ExecuteAction();
			
			Finish();
		}
		
        void ExecuteAction()
        {

            if (!automaticallySyncScene.IsNone)
            {
                automaticallySyncScene.Value = PhotonNetwork.AutomaticallySyncScene;
            }
            
            if (PhotonNetwork.AutomaticallySyncScene && isAutomaticallySyncSceneEvent != null)
            {
                Fsm.Event(isAutomaticallySyncSceneEvent);
            }else if (!PhotonNetwork.AutomaticallySyncScene && isNotAutomaticallySyncSceneEvent != null)
            {
	            Fsm.Event(isNotAutomaticallySyncSceneEvent);
            }
                

        }

	}
}