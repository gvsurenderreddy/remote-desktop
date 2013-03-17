using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDataLayer
{
    public interface IMouseCommands
    {
        void Execute(object sender, MouseActionEventArgs args);
        void BindCommands();
    }
}
