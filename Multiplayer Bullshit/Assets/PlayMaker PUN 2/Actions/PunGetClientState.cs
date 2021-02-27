// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Gets Photon networking client connection state")]
    [HelpUrl("")]
	public class PunGetClientState : PunActionBase
	{

        [Tooltip("The current client state")]
        [UIHint(UIHint.Variable)]
        [ObjectType(typeof(ClientState))]
        public FsmEnum clientState;

        [Tooltip("The previous client state")]
        [UIHint(UIHint.Variable)]
        [ObjectType(typeof(ClientState))]
        public FsmEnum previousClientState;

        [Tooltip("Event sent when client state changed")]
        public FsmEvent OnClientStateChanged;

		[Tooltip("Repeat every frame. Useful for watching the network state over time.")]
		public bool everyFrame;

		public override void Reset()
		{
            clientState = ClientState.Disconnected;
            previousClientState = ClientState.Disconnected;

            OnClientStateChanged = null;
            everyFrame = false;
		}

		public override void OnEnter()
		{
            PhotonNetwork.NetworkingClient.StateChanged += NetworkingClient_StateChanged;

            previousClientState.Value = PhotonNetwork.NetworkClientState;
            clientState.Value = PhotonNetwork.NetworkClientState;

            if (!everyFrame)
			{
                Finish();
			}
        }

        public override void OnExit()
        {
            PhotonNetwork.NetworkingClient.StateChanged -= NetworkingClient_StateChanged;
        }

        void NetworkingClient_StateChanged(ClientState arg1, ClientState arg2)
        {
            previousClientState.Value = arg1;
            clientState.Value = arg2;

            Fsm.Event(OnClientStateChanged);
        }

	}
}