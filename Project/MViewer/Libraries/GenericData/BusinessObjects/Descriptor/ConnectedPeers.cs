﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace GenericObjects
{
    public class PeerStates
    {
        public GenericEnums.SessionState VideoSessionState { get; set; }
        public GenericEnums.SessionState AudioSessionState { get; set; }
        public GenericEnums.SessionState RemotingSessionState { get; set; }
    }
}
