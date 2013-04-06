using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericObjects;
using Utils;

namespace StrategyPattern
{
    public interface IRoomCommandInvoker
    {
        void PerformCommand(object sender, RoomActionEventArgs args);
    }
}
