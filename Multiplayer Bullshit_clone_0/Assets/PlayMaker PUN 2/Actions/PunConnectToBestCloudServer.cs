// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using UnityEngine;
using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Connect to Photon Network: \n" +
		"Connect to the configured Photon server: Reads NetworkingPeer.serverSettingsAssetPath and connects to cloud or your own server. \n" +
        "Uses: PhotonNetwork.ConnectToBestCloudServer()")]
	public class PunConnectToBestCloudServer : PunActionBase
	{
		
		[Tooltip("The AppId. Leave to none or empty to use the one from the Server Settings")]
		public FsmString appIdRealtime;

        public FsmBool resetBestRegionInPref;

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
            resetBestRegionInPref = false;
            appIdRealtime = new FsmString(){UseVariable=true};

            result = null;
            willProceed = null;
            willNotProceed = null;
        }

		public override void OnEnter()
		{
			// reset authentication failure properties.
			PlayMakerPhotonProxy.lastAuthenticationDebugMessage = string.Empty;
			PlayMakerPhotonProxy.lastAuthenticationFailed=false;

            bool _result;

            #if !(UNITY_WINRT || UNITY_WP8 || UNITY_PS3 || UNITY_WIIU)
            if (!appIdRealtime.IsNone || string.IsNullOrEmpty(appIdRealtime.Value))
            {
                PhotonNetwork.NetworkingClient.AppId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;
            }
            else
            {
                PhotonNetwork.NetworkingClient.AppId = appIdRealtime.Value;
            }

            if (resetBestRegionInPref.Value)
            {
                ServerSettings.ResetBestRegionCodeInPreferences();
            }

            _result = PhotonNetwork.ConnectToBestCloudServer();

            #else
                Debug.Log("Connect to Best Server is not available on this platform");
            #endif


            if (!result.IsNone)
            {
                result.Value = _result;
            }

            Fsm.Event(_result ? willProceed : willNotProceed);

            Finish();
		}
		
		public override string ErrorCheck()
		{
			#if (UNITY_WINRT || UNITY_WP8 || UNITY_PS3 || UNITY_WIIU)
				return "Connect to Best Server is not available on this platform, the normal connection protocol will be used instead.";
			#endif	
			
			return string.Empty;
		}

	}
}