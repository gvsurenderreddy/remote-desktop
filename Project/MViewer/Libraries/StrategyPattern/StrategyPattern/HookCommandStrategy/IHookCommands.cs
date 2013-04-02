using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using GenericObjects;

namespace StrategyPattern
{
    public interface IHookCommands
    {
        void Execute(object sender, RemotingCommandEventArgs args);
    }
}
