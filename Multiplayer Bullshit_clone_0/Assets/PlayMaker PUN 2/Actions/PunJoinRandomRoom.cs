// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using UnityEngine;
using System.Collections;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Joins any available room but will fail if none is currently available.\n" +
	 	"Optionnally define expected custom properties to match, max Players and matchmkaing mode: http://doc.exitgames.com/photon-cloud/MatchmakingAndLobby/#cat-reference")]
	[HelpUrl("")]
	public class PunJoinRandomRoom : PunActionBase
    { 

        [ObjectType(typeof(MatchmakingMode))]
        public FsmEnum matchMakingMode;

		[Tooltip("Max Player in rooms to filter. Leave to 0 if you don't want to filter by players numbers in rooms")]
		public FsmInt maxPlayer;
		

		[ActionSection("Expected room properties")]
		
		[Tooltip("room properties to filter rooms before picking a random one.")]
		[CompoundArray("Custom Properties", "property", "value")]
		public FsmString[] customPropertyKeys;
		[Tooltip("Values related to the properties")]
		public FsmVar[] customPropertiesValues;
		
		
		[ActionSection("Lobby properties")]
		
		[Tooltip("LobbyFilters. Leave to none to ingore it")]
		[UIHint(UIHint.Variable)]
		public FsmString sqlLobbyFilter;

        [ActionSection("Result")]
        [UIHint(UIHint.Variable)]
        [Tooltip("false if the request will not be attempted. True will attempt request")]
        public FsmBool result;

        [Tooltip("Event to send if the request will be attempted")]
        public FsmEvent willProceed;

        [Tooltip("Event to send if the request will not be attempted")]
        public FsmEvent willNotProceed;

        public override void Reset()
		{
			matchMakingMode = MatchmakingMode.RandomMatching;
			
			maxPlayer = new FsmInt() {UseVariable=true};
			
			//typedLobby = TypedLobby.Default;
				
			sqlLobbyFilter = new FsmString() {UseVariable=true};
			
			customPropertyKeys = new FsmString[0];
			customPropertiesValues = new FsmVar[0];

            result = null;
            willProceed = null;
            willNotProceed = null;
        }
		

		public override void OnEnter()
		{
			bool withExpections = false;
			int _maxPlayer = 0;
			
			
			ExitGames.Client.Photon.Hashtable _props = new ExitGames.Client.Photon.Hashtable();

			int i = 0;
			foreach(FsmString _prop in customPropertyKeys)
			{
				withExpections = true;
				_props[_prop.Value] =  PlayMakerUtils.GetValueFromFsmVar(this.Fsm,customPropertiesValues[i]);
				i++;
			}


			if ( (! maxPlayer.IsNone) || maxPlayer.Value>0)
			{
				_maxPlayer = maxPlayer.Value;
				withExpections = true;
			}
			
			if (customPropertyKeys.Length>0)
			{
				withExpections =  true;
			}
			
			if ((MatchmakingMode)matchMakingMode.Value != MatchmakingMode.FillRoom)
			{
				withExpections =  true;
			}

            bool _result;

			if (withExpections)
			{
				_result = PhotonNetwork.JoinRandomRoom(_props,(byte)_maxPlayer, (MatchmakingMode)matchMakingMode.Value, TypedLobby.Default,sqlLobbyFilter.Value);
			}else{
                _result = PhotonNetwork.JoinRandomRoom();
			}

            if (!result.IsNone)
            {
                result.Value = _result;
            }

            Fsm.Event(_result ? willProceed : willNotProceed);

            Finish();
		}
	}
}