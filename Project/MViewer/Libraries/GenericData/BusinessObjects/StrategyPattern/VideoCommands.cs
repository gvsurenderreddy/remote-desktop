using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericDataLayer;
using Utils;

namespace GenericDataLayer
{
    public class VideoCommands : CommandBase
    {
        Dictionary<GenericEnums.SignalType, Delegates.CommandDelegate> _videoCommands;

        // todo: add delegate handlers
        public Delegates.CommandDelegate StartVideoChat;
        public Delegates.CommandDelegate StopVideChat;
        public Delegates.CommandDelegate PauseVideoChat;
        public Delegates.CommandDelegate ResumeVideoChat;


        public override void BindCommands()
        {
            _videoCommands = new Dictionary<GenericEnums.SignalType, Delegates.CommandDelegate>();
            _videoCommands.Add(GenericEnums.SignalType.Start, StartVideoChat);
            _videoCommands.Add(GenericEnums.SignalType.Stop, StopVideChat);
            _videoCommands.Add(GenericEnums.SignalType.Pause, PauseVideoChat);
            _videoCommands.Add(GenericEnums.SignalType.Resume, ResumeVideoChat);
        }

        protected override void PerformCommand(object sender, RoomActionEventArgs args)
        {
            // todo: perform specific video command
            _videoCommands[args.SignalType].Invoke(sender, args);
        }
    }
}
