// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Create a room With advanced settings.Please refer to Photon documentation.")]
	[HelpUrl("")]
	public class PunCreateRoomAdvanced : FsmStateAction
	{
		[Tooltip("The room Name")]
		public FsmString roomName;
		
		[Tooltip("Is the room visible")]
		public FsmBool isVisible;
		
		[Tooltip("Is the room open")]
		public FsmBool isOpen;

		[Tooltip("Player Time to Live. If a client disconnects, this actor is inactive first and removed after this timeout. In milliseconds.")]
		public FsmInt playerTimeToLive;
			
		[Tooltip("Max numbers of players for this room.")]
		[TitleAttribute("Max Number Of Players")]
		public FsmInt maxNumberOfPLayers;

		[Tooltip("Time To Live (TTL) for a room when the last player leaves. Keeps room in memory for case a player re-joins soon. In milliseconds.")]
		public FsmInt emptyRoomTimeToLive;

		[Tooltip("Clean up on room leaving. Leave to 'none' to use the default value")]
		public FsmBool cleanupCacheOnLeave;

		[Tooltip("Properties get deleted, when null gets assigned as value. Leave to 'none' to use the default value")]
		public FsmBool deleteNullProperties;
		
		[Tooltip("Tells the server to skip room events for joining and leaving players")]
		public FsmBool suppressRoomEvents;

		
		[Tooltip("By default, property changes are sent back to the client that's setting them to avoid de-sync when properties are set concurrently.\n" +
		         "Leave to 'none' to use the default value")]
		public FsmBool BroadcastPropsChangeToAll;
		

		[Tooltip("Defines if the UserIds of players get 'published' in the room. Useful for FindFriends, if players want to play another game together.")]
		public FsmBool publishUserId;

		[Tooltip("Informs the server of the expected plugin setup.")]
		public FsmString[] plugins;

		[Tooltip("The lobby type for this room")]
		public TypedLobbyProperty lobby;

		[Tooltip("The expected Users list, by UserId")]
		[ArrayEditor(VariableType.GameObject)]
		public FsmArray expectedUsers;
		

		[ActionSection("Custom Properties")]
	
			
		[CompoundArray("Count", "Key", "Value")]
		[Tooltip("The Custom Property to set")]
		public FsmString[] customPropertyKey;
		[RequiredField]
		[Tooltip("Value of the property")]
		public FsmVar[] customPropertyValue;
		
		[ActionSection("Lobby custom Properties")]
		[Tooltip("Properties listed in the lobby.")]
		public FsmString[] lobbyCustomProperties;


		private string[] _expectedUsers = null;
		
		public override void Reset()
		{
			roomName  = new FsmString() {UseVariable=true};
			isVisible = true;
			isOpen = true;

			playerTimeToLive  = new FsmInt() {UseVariable=true};
			emptyRoomTimeToLive = new FsmInt() {UseVariable=true};

			suppressRoomEvents = new FsmBool() {UseVariable=true};

			publishUserId = new FsmBool() {UseVariable=true};

			plugins = null;

			cleanupCacheOnLeave = new FsmBool() {UseVariable=true};
			
			deleteNullProperties = new FsmBool() {UseVariable=true};
			
			BroadcastPropsChangeToAll = new FsmBool() {UseVariable=true};
				 
			expectedUsers = new FsmArray() {UseVariable = true};
			
			maxNumberOfPLayers = 4;
			customPropertyKey = null;
			customPropertyValue = null;
			lobbyCustomProperties = null;
			
			lobby = new TypedLobbyProperty();


			
		}

		public override void OnEnter()
		{
			
		
			string _roomName = null;
			if ( ! string.IsNullOrEmpty(roomName.Value) )
			{
				_roomName = roomName.Value;
			}
				

			ExitGames.Client.Photon.Hashtable _props = new ExitGames.Client.Photon.Hashtable();
			
			int i = 0;
			foreach(FsmString _prop in customPropertyKey)
			{
				_props[_prop.Value] =  PlayMakerUtils.GetValueFromFsmVar(this.Fsm,customPropertyValue[i]);
				i++;
			}
			
			
			string[] lobbyProps = new string[lobbyCustomProperties.Length];
			
			int j = 0;
			foreach(FsmString _visibleProp in lobbyCustomProperties)
			{
				lobbyProps[j] = _visibleProp.Value;
				j++;
			}

			RoomOptions _options = new RoomOptions();
			_options.MaxPlayers =  (byte)maxNumberOfPLayers.Value;
			_options.IsVisible = isVisible.Value;
			_options.IsOpen = isOpen.Value;
			_options.CustomRoomProperties = _props;
			_options.CustomRoomPropertiesForLobby = lobbyProps;
			
			if (!playerTimeToLive.IsNone)
			{
				_options.PlayerTtl = playerTimeToLive.Value;
			}

			if (!emptyRoomTimeToLive.IsNone)
			{
				_options.EmptyRoomTtl = emptyRoomTimeToLive.Value;
			}

			if (!cleanupCacheOnLeave.IsNone)
			{
				_options.CleanupCacheOnLeave = cleanupCacheOnLeave.Value;
			}

			
			if (!suppressRoomEvents.IsNone)
			{
				_options.SuppressRoomEvents = suppressRoomEvents.Value;
			}
			
			if (!deleteNullProperties.IsNone)
			{
				_options.DeleteNullProperties = deleteNullProperties.Value;
			}
			
			if (!BroadcastPropsChangeToAll.IsNone)
			{
				_options.BroadcastPropsChangeToAll = BroadcastPropsChangeToAll.Value;
			}
			

			if (!publishUserId.IsNone)
			{
				_options.PublishUserId = publishUserId.Value;
			}

			if (!expectedUsers.IsNone)
			{
				_expectedUsers = expectedUsers.stringValues;
			}
			else
			{
				_expectedUsers = null;
			}

			if (plugins.Length>0)
			{
				string[] _plugins = new string[plugins.Length];

				int k = 0;
				foreach(FsmString _fsmstring in plugins)
				{
					_plugins[k] = _fsmstring.Value;
					k++;
				}

				_options.Plugins = _plugins;
			}



			PhotonNetwork.CreateRoom(_roomName,_options,lobby.GetTypedLobby(),_expectedUsers);
			
			
			Finish();
		}

	}
}