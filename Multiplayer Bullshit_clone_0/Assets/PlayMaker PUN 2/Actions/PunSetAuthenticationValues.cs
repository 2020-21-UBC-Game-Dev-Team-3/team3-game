// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Defines Authentication values to use for connection ( using PhotonNetworkConnectUsingSettings or PhotonNetworkConnectManually).\n" +
		"Failed Custom Authentication will fire a global Photon event 'CUSTOM AUTHENTICATION FAILED' event.")]
	[HelpUrl("")]
	public class PunSetAuthenticationValues : FsmStateAction
	{
		[Tooltip("The type of custom authentication provider that should be used. Set to 'None' to turn off.")]
		[ObjectType(typeof(CustomAuthenticationType))]
		public FsmEnum authenticationType;

		[Tooltip("the User Id")]
		[RequiredField]
		public FsmString userId;
		
		[Tooltip("Authentication Parameters")]
		[CompoundArray("Authentication Parameters", "key", "value")]
		public FsmString[] authParameterKeys;
		[UIHint(UIHint.Variable)]
		public FsmString[] authParameterValues;
		
		
		
		[Tooltip("Sets the data to be passed-on to the auth service via POST. Empty string will set AuthPostData to null.")]
		public FsmString authPostData;
		
		public override void Reset()
		{
			authenticationType = CustomAuthenticationType.Custom;
			userId = new FsmString(){UseVariable = true};
			
			authPostData = new FsmString(){UseVariable=true};

			authParameterKeys = null;
			authParameterValues = null;
			
		}

		public override void OnEnter()
		{
			ExecuteAction();

			Finish();
		}

		void ExecuteAction()
		{
			PhotonNetwork.AuthValues = new AuthenticationValues();
			
			PhotonNetwork.AuthValues.AuthType = (CustomAuthenticationType)authenticationType.Value;

			if (!userId.IsNone)
			{
				PhotonNetwork.AuthValues.UserId = userId.Value;
			}

			if (!authPostData.IsNone)
			{
				PhotonNetwork.AuthValues.SetAuthPostData(authPostData.Value);
			}

			// get the paremeters
			int i = 0;
			foreach(FsmString key in authParameterKeys)
			{
					PhotonNetwork.AuthValues.AddAuthParameter(key.Value, authParameterValues[i].Value);
				i++;
			}
		}
	}
}