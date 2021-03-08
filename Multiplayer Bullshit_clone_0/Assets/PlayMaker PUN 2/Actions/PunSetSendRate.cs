// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Defines how many times per second PhotonNetwork should send a package. Default is 20.\n" +
		"If you change this, do not forget to also change 'sendRateOnSerialize'.. \n" +
	         "Less packages are less overhead but more delay.Setting the sendRate to 50 will create up to 50 packages per second (which is a lot!).\n" +
	         "Keep your target platform in mind: mobile networks are slower and less reliable.")]
	public class PunSetSendRate : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Defines how many times per second PhotonNetwork should send a package, default is 20")]
		public FsmInt sendRate;

		public override void Reset()
		{
			sendRate = 20;
		}
		
		public override void OnEnter()
		{
			ExecuteAction();
			
			Finish();
		}
		
		void ExecuteAction()
		{
			if(!sendRate.IsNone)
			{
				PhotonNetwork.SendRate = sendRate.Value;
			}else
			{
				LogError("sendRate is undefined");
			}
		}
	}
}
