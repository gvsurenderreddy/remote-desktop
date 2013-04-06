using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericObjects;

namespace StrategyPattern
{
    public interface IHookCommandInvoker
    {
        void PerformCommand(object sender, RemotingCommandEventArgs args);
    }
}
