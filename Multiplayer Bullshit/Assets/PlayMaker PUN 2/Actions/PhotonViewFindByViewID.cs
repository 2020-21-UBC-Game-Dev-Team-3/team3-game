// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2.Actions
{
	[ActionCategory("Photon")]
	[Tooltip("Find the GameObject with a photon NetworkView with a given NetworkView ID.")]
	//[HelpUrl("")]
	public class PhotonViewFindByViewID : PunActionBase
	{

		[Tooltip("The PhotonView ID as int to find")]
		public FsmInt ID;

		[Tooltip("The PhotonView ID as string to find. Leave to false for no effect")]
		public FsmString IdAsString;

		[ActionSection("result")]
		[UIHint(UIHint.Variable)]
		[Tooltip("The Game Object with the PhotonView of the given ID.")]
		public FsmGameObject gameObject;

		[Tooltip("Set to true if a GameObject is found with that photonView ID")]
		[UIHint(UIHint.Variable)]
		public FsmBool GameObjectFound;

		[Tooltip("Event sent if a GameObject is found with that photonView ID")]
		public FsmEvent GameObjectFoundEvent;

		[Tooltip("Event sent if bo GameObject is found with that photonView ID")]
		public FsmEvent GameObjectNotFoundEvent;

		public override void Reset()
		{
			gameObject = null;
			ID = null;
			IdAsString = new FsmString(){UseVariable=true};
			GameObjectFound = null;
			GameObjectFoundEvent = null;
			GameObjectNotFoundEvent = null;
		}
		
		public override void OnEnter()
		{
			int _id = -1;

			bool ok = false;

			if (!IdAsString.IsNone)
			{
				ok = int.TryParse(IdAsString.Value,out _id);
			}

			if (!ok)
			{
				_id = ID.Value;
			}

			PhotonView _pv = PhotonView.Find(_id);

			bool _found = _pv!=null;
			GameObjectFound.Value = _found;

			if (!_found)
			{
				gameObject.Value = null;
				Fsm.Event(GameObjectNotFoundEvent);
			}else{
				gameObject.Value = _pv.gameObject;
				Fsm.Event(GameObjectFoundEvent);
			}

			Finish();
		}
	}
}