using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace GenericDataLayer
{
    public class ControllerHookCommandHandlers
    {
        public Delegates.HookCommandDelegate Mouse { get; set; }
        public Delegates.HookCommandDelegate Keyboard { get; set; }
    }
}
