using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericObjects;

namespace StrategyPattern
{
    public interface IHookCommands
    {
        void Execute(object sender, EventArgs args);
    }
}
