using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace GenericDataLayer
{
    public class RemotingCommands : CommandBase
    {
        public Delegates.CommandDelegate StartRemoting;
        public Delegates.CommandDelegate StopRemoting;
        public Delegates.CommandDelegate PauseRemoting;
        public Delegates.CommandDelegate ResumeRemoting;

        public override void BindCommands()
        {
            _commands = new Dictionary<GenericEnums.SignalType, Delegates.CommandDelegate>();
            _commands.Add(GenericEnums.SignalType.Start, StartRemoting);
            _commands.Add(GenericEnums.SignalType.Stop, StopRemoting);
            _commands.Add(GenericEnums.SignalType.Start, PauseRemoting);
            _commands.Add(GenericEnums.SignalType.Resume, ResumeRemoting);
        }
    }
}
