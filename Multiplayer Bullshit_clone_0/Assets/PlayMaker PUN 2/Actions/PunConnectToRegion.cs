// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using UnityEngine;
using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Connect to Photon Network region: \n" +
		"Connect to the configured Photon server: Reads NetworkingPeer.serverSettingsAssetPath and connects to cloud or your own server. \n" +
		"Uses: Connect(string serverAddress, int port, string uniqueGameID)")]
	public class PunConnectToRegion : PunActionBase
	{

		[Tooltip("The region")]
		public FsmString region;

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
			region = null;
            result = null;
            willProceed = null;
            willNotProceed = null;
        }

		public override void OnEnter()
		{
			// reset authentication failure properties.
			PlayMakerPhotonProxy.lastAuthenticationDebugMessage = string.Empty;
			PlayMakerPhotonProxy.lastAuthenticationFailed=false;

		    bool _result = PhotonNetwork.ConnectToRegion(region.Value);
            if (!result.IsNone)
            {
                result.Value = _result;
            }

            Fsm.Event(_result ? willProceed : willNotProceed);

            Finish();
		}

	}
}