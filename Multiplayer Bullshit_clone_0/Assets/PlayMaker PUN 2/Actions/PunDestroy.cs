// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Destroys given GameObject. This GameObject must've been instantiated using PhotonNetworkInstantiate and must have a PhotonView at it's root.")]
	[HelpUrl("")]
	public class PhotonNetworkDestroy : PunActionBase
	{
		[RequiredField]
		[Tooltip("Destroys this GameObject")]
		public FsmGameObject gameObject;

		public override void Reset()
		{
			gameObject = null;
		}

		public override void OnEnter()
		{
			doDestroy();
			
			Finish();
		}
		
		
		void doDestroy()
		{
			var go = gameObject.Value;

			if (go != null)
			{
				PhotonNetwork.Destroy(go);
			}	
		}
	
	}
}