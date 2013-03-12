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
        // todo: add delegate handlers
        public Delegates.CommandDelegate StartVideoChat;
        public Delegates.CommandDelegate StopVideChat;
        public Delegates.CommandDelegate PauseVideoChat;
        public Delegates.CommandDelegate ResumeVideoChat;


        // todo: move these to audio class
        public Delegates.CommandDelegate StartAudioChat;
        public Delegates.CommandDelegate StopAudioChat;


        public override void BindCommands()
        {
            _commands = new Dictionary<GenericEnums.SignalType, Delegates.CommandDelegate>();
            _commands.Add(GenericEnums.SignalType.Start, StartVideoChat);
            _commands.Add(GenericEnums.SignalType.Stop, StopVideChat);
            _commands.Add(GenericEnums.SignalType.Pause, PauseVideoChat);
            _commands.Add(GenericEnums.SignalType.Resume, ResumeVideoChat);
        }
    }
}
