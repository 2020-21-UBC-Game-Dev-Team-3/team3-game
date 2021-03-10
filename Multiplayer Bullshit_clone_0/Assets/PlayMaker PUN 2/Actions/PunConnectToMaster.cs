// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Connect to the photon server by address, port, appID" +
		"This method is used by ConnectUsingSettings which applies values from the settings file.)")]
	[HelpUrl("")]
	public class PunConnectToMaster : PunActionBase
	{
		[Tooltip("The master server's address (either your own or Photon Cloud address).")]
		public FsmString serverAddress;
		
		[Tooltip("The master server's port to connect to.")]
		public FsmInt port;
		
		[Tooltip("Your application ID (Photon Cloud provides you with a GUID for your game).")]
		public FsmString applicationID;

        [ActionSection("Result")]
        [UIHint(UIHint.Variable)]
        [Tooltip("false if the connection will not be attempted. True will attempt connection")]
        public FsmBool result;

        [Tooltip("Event to send if the connection will be attempted")]
        public FsmEvent willProceed;

        [Tooltip("Event to send if the connection will not be attempted")]
        public FsmEvent willNotProceed;


        public override void Reset()
		{
			serverAddress  = "app-eu.exitgamescloud.com";
			port = 5055;
			applicationID = "YOUR APP ID";

            result = null;
            willProceed = null;
            willNotProceed = null;
        }

		public override void OnEnter()
		{
            // reset authentication failure properties.
            PlayMakerPhotonProxy.lastAuthenticationDebugMessage = string.Empty;
            PlayMakerPhotonProxy.lastAuthenticationFailed = false;

            bool _result = PhotonNetwork.ConnectToMaster(serverAddress.Value,port.Value,applicationID.Value);
            if (!result.IsNone)
            {
                result.Value = _result;
            }

            Fsm.Event(_result ? willProceed : willNotProceed);

            Finish();
		}

	}
}