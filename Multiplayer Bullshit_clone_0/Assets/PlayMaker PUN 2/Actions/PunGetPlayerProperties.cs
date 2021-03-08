// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License


using Photon.Realtime;
using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Get built in and custom properties of a Photon player.")]
	[HelpUrl("")]
	public class PhotonNetworkGetPlayersProperties : PunActionBase
    {

        [Tooltip("The Photon player")]
        [RequiredField]
        public PlayerReferenceProperty player;

        [ActionSection("Builtin Properties")]
		[Tooltip("The player's NickName")]
		[UIHint(UIHint.Variable)]
		public FsmString nickname;

		[Tooltip("The player's ID")]
		[UIHint(UIHint.Variable)]
		public FsmInt actorNumber;

		[Tooltip("Then player's userId")]
		[UIHint(UIHint.Variable)]
		public FsmString userId;

		[Tooltip("The player's inactive state")]
		[UIHint(UIHint.Variable)]
		public FsmBool isInactive;

		[Tooltip("The Player's Local flag")]
		[UIHint(UIHint.Variable)]
		public FsmBool isLocal;

		[Tooltip("The player's MasterClient flag")]
		[UIHint(UIHint.Variable)]
		public FsmBool isMasterClient;

		[Tooltip("Custom Properties you have assigned to this player.")]
		[CompoundArray("Player's Custom Properties", "property", "value")]
		public FsmString[] customPropertyKeys;
		[UIHint(UIHint.Variable)]
		public FsmString[] customPropertiesValues;

		private Player _player;

		public override void Reset()
		{
            nickname = null;
            actorNumber = null;
            userId = null;
            isInactive = null;
            isLocal = null;
            isMasterClient = null;

			customPropertyKeys = null;
			customPropertiesValues = null;

		}

        public override void OnEnter()
        {
            ExecuteAction();
            Finish();
        }

        void ExecuteAction()
        { 
            _player = player.GetPlayer(this);

            if (_player == null)
            {
                return;
            }

            if (!nickname.IsNone) nickname.Value = _player.NickName;
            if (!actorNumber.IsNone) actorNumber.Value = _player.ActorNumber;
            if (!userId.IsNone) userId.Value = _player.UserId;
            if (!isInactive.IsNone) isInactive.Value = _player.IsInactive;
            if (!isLocal.IsNone) isLocal.Value = _player.IsLocal;
            if (!isMasterClient.IsNone) isMasterClient.Value = _player.IsMasterClient;


            customPropertiesValues = new FsmString[customPropertyKeys.Length];


            // get the custom properties
            int i = 0;
			foreach(FsmString key in customPropertyKeys)
			{
				if (_player.CustomProperties.ContainsKey(key.Value) && ! customPropertiesValues[i].IsNone)
				{
					customPropertiesValues[i] = (string)_player.CustomProperties[key.Value];
				}
				i++;
			}

		}
		
	}
}