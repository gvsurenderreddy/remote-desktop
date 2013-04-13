using Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericObjects
{
    public class ActiveRooms : ActiveRoomsBase
    {
        public string AudioRoomIdentity { get; set; }
        public string VideoRoomIdentity { get; set; }
        public string RemotingRoomIdentity { get; set; }

    }
}
