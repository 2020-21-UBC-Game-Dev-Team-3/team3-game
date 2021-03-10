// (c) Copyright HutongGames, LLC 2010-2019. All rights reserved.
// Author jean@hutonggames.com
// This code is licensed under the MIT Open source License

using System.Collections.Generic;

using Photon.Pun;
using Photon.Realtime;

namespace HutongGames.PlayMaker.Pun2
{
    public class PlayMakerPhotonRoomListProxy : MonoBehaviourPunCallbacks
    {
        Dictionary<string, RoomInfo> RoomList = new Dictionary<string, RoomInfo>();

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            foreach (RoomInfo room in roomList)
            {
                if (RoomList.ContainsKey(room.Name))
                {
                    if (room.RemovedFromList)
                    {
                        // we delete the entry
                        RoomList.Remove(room.Name);
                    }
                    else
                    {
                        // we update the entry
                        RoomList[room.Name] = room;
                    }
                }
                else
                {
                    if (!room.RemovedFromList)
                    {
                        // we create the entry
                        RoomList[room.Name] = room;
                    }
                }
            }


        }
    }
}