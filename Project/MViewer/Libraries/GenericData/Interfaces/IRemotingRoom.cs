using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDataLayer
{
    public interface IRemotingRoom
    {
        void ShowMouseCapture(byte[] capture);
        void ShowScreenCapture(byte[] capture);
    }
}
