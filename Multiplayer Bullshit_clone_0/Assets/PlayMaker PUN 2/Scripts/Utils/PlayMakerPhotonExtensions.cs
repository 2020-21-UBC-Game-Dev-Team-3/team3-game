// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun;

namespace HutongGames.PlayMaker.Pun2
{
    public static class PlayMakerPhotonExtensions
    {

        public static Player FindByUserID(this Player player, string userId)
        {
            foreach (Player _player in PhotonNetwork.PlayerList)
            {
                if (_player.UserId == userId)
                {
                    return _player;
                }
            }
            return null;
        }

        public static Player FindPlayerByNumber(this Room room, int number)
        {
            if (room == null)
            {
                return null;
            }

            foreach (Player _player in room.Players.Values)
            {
                if (_player.GetPlayerNumber() == number)
                {
                    return _player;
                }
            }
            return null;
        }

        public static Player FindPlayerByNickname(this Room room, string nickname)
        {
            if (room == null)
            {
                return null;
            }

            foreach (Player _player in room.Players.Values)
            {
                if (_player.NickName == nickname)
                {
                    return _player;
                }
            }
            return null;
        }
    }
}