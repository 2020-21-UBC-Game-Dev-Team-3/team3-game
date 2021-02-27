// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Test if the Photon network is the masterClient. Note this can change during the session.")]
	[HelpUrl("")]
	public class PunGetIsMasterClient : PunActionBase
	{
		
		[UIHint(UIHint.Variable)]
		[Tooltip("True if the Photon network is currently the masterClient.")]
		public FsmBool isMasterClient;
		
		[Tooltip("Send this event if the Photon network is the masterClient.")]
		public FsmEvent isMasterClientEvent;
		
		[Tooltip("Send this event if the Photon network view is NOT the master client.")]
		public FsmEvent isNotMasterClientEvent;

        [Tooltip("Event sent when this property changed")]
        public FsmEvent onValueChanged;

        [Tooltip("Repeat every frame. Useful for watching the value change over time.")]
        public bool everyFrame;


        bool _lastValue;
        bool _isMasterClient;

        public override void Reset()
		{
			isMasterClient = null;
			isMasterClientEvent = null;
			isNotMasterClientEvent = null;

            onValueChanged = null;
            everyFrame = false;

        }

		public override void OnEnter()
		{
			checkIsMasterClient(true);

            if (!everyFrame)
            {
                Finish();
            }

        }

        public override void OnUpdate()
        {
            checkIsMasterClient();
        }

        public override string ErrorCheck()
        {
            if (onValueChanged!=null && !everyFrame)
            {
                return "Everyframe needs to be true for onValueChanged to have a chance to be sent";
            }
            return string.Empty;
        }

        void checkIsMasterClient(bool firstTime = false)
        {
            _isMasterClient = PhotonNetwork.IsMasterClient;

            isMasterClient.Value = _isMasterClient;

            if (firstTime)
            {
                if (_isMasterClient)
                {
                    if (isMasterClientEvent != null)
                    {
                        Fsm.Event(isMasterClientEvent);
                    }
                }
                else
                {
                    if (isNotMasterClientEvent != null)
                    {
                        Fsm.Event(isNotMasterClientEvent);
                    }
                }
            }



            if (!firstTime && _lastValue != _isMasterClient)
            {
                if (onValueChanged != null)
                {
                    Fsm.Event(onValueChanged);
                }
            }

            _lastValue = _isMasterClient;
        }

	}
}