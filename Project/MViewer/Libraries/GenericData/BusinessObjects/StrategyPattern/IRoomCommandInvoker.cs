using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace GenericDataLayer
{
    public interface IRoomCommandInvoker
    {
        void PerformCommand(object sender, RoomActionEventArgs args);
    }
}
