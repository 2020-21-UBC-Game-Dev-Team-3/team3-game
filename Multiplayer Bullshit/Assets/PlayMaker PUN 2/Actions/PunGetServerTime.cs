// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get the Photon network time, synched with the server. This time value depends on the server's Environment.TickCount. It is different per server" +
		"but inside a Room, all clients should have the same value (Rooms are on one server only)." +
		"" +
		"This is not a DateTime!  Use this value with care:" +
		"It can start with any positive value." +
		"It will 'wrap around' from 4294967.295 to 0!")]
	[HelpUrl("")]
	public class PhotonNetworkGetTime : FsmStateAction
	{
		[Tooltip("The Photon network time")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat time;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			time = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			ExecuteAction();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			ExecuteAction();
		}

		void ExecuteAction()
		{
			time.Value = (int)PhotonNetwork.Time;
		}
	}
}