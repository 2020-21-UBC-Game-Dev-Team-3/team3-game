// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Realtime;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Retrieve the disconnect cause of the last photon message (OnConnectionFail, OnFailedToConnectToPhoton).")]
	[HelpUrl("")]
	public class PunGetEventData : PunActionBase
	{

        public PunCallbacksWithData pun2Event;

        [Tooltip("The disconnect cause")]
		[UIHint(UIHint.Variable)]
        [ObjectType(typeof(DisconnectCause))]
		public FsmEnum cause;

		[Tooltip("Send this event if the disconnection cause was found.")]
		public FsmEvent successEvent;
		
		[Tooltip("Send this event if the disconnection cause was not found.")]
		public FsmEvent failureEvent;

        bool _ok;

		public override void Reset()
		{
            pun2Event = PunCallbacksWithData.OnDisconnected;

            cause = null;
			
			successEvent = null;
			failureEvent = null;
		}

		public override void OnEnter()
        { 
            _ok = false;


            if (pun2Event == PunCallbacksWithData.OnDisconnected)
            {
                if (PlayMakerPunCallbacksProxy.Instance.LastCallback == (PunCallbacks)pun2Event)
                {
                    cause.Value = PlayMakerPunCallbacksProxy.Instance.lastDisconnectCause;
                    _ok = true;
                }
            }
           

			if (_ok)
			{
				Fsm.Event(successEvent);
			}else{
				Fsm.Event(failureEvent);
			}
			Finish();
		}
	}
}