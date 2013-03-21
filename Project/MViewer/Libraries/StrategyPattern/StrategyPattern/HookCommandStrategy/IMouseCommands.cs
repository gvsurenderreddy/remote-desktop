using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyPattern
{
    public interface IMouseCommands
    {
        void Execute(object sender, EventArgs args);
        void BindCommands();
    }
}
