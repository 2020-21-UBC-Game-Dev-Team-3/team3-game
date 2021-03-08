// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Connect to Photon Network using Server Settings: \n" +
		"Connect to the configured Photon server: Reads NetworkingPeer.serverSettingsAssetPath and connects to cloud or your own server. \n" +
        "Uses: PhotonNetwork.ConnectUsingSettings()")]
	[HelpUrl("")]
	public class PunConnectUsingSettings : PunActionBase
	{

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
            result = null;
            willProceed = null;
            willNotProceed = null;
        }

        public override void OnEnter()
		{
			// reset authentication failure properties.
			PlayMakerPhotonProxy.lastAuthenticationDebugMessage = string.Empty;
			PlayMakerPhotonProxy.lastAuthenticationFailed=false;

            bool _result = PhotonNetwork.ConnectUsingSettings();
            if (!result.IsNone)
            {
                result.Value = _result;
            }

            Fsm.Event(_result ? willProceed : willNotProceed);

            //   this.header.NetworkOperationPending = true;

            Finish();
		}
	}
}