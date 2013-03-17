using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericDataLayer;

namespace UIControls
{
    public interface IMouseCommandInvoker
    {
        void PerformCommand(object sender, MouseActionEventArgs args);
    }
}
