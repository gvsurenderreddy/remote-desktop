using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StrategyPattern
{
    public interface IMouseCommands
    {
        void Execute(object sender, EventArgs args);
        void BindCommands();
    }
}
