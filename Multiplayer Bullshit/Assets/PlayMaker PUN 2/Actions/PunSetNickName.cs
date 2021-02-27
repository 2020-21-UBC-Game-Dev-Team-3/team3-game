// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Set the name of the Photon player.\n" +
	         "Synchronize the player's nickname with everyone in the room(s) you enter or in the room you are already in.\n" +
	         "The NickName is just a nickname and does not have to be unique or backed up with some account.")]
	[HelpUrl("")]
	public class PunSetNickName : FsmStateAction
	{
		[Tooltip("The Photon player nickName")]
		[RequiredField]
		public FsmString nickName;
		
		public override void Reset()
		{
			nickName = null;
		}

		public override void OnEnter()
		{
			if (!nickName.IsNone)
			{
				PhotonNetwork.NickName = nickName.Value;
			}
			else
			{
				LogError("NickName undefined");
			}

			Finish();
		}

	}
}