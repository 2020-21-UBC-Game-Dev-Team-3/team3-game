// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using System;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Pun2.Actions
{
    [Serializable]
    public class TypedLobbyProperty
    {

        [Tooltip("The lobby name, leave to none for no effect")]
        public FsmString name;

        [Tooltip("the lobby type. Leave to none for no effect")]
        [ObjectType(typeof(LobbyType))]
        public FsmEnum type;

        public TypedLobbyProperty()
        {
            name = new FsmString() { UseVariable = true };
            type = new FsmEnum() { UseVariable = true};
        }


        public TypedLobby GetTypedLobby()
        {
            TypedLobby _t = TypedLobby.Default;

            if (!name.IsNone)
            {
                _t.Name = name.Value;
            }

            if (!type.IsNone)
            {
                _t.Type = (LobbyType)type.Value;
            }

            return _t;
        }
    }
}
