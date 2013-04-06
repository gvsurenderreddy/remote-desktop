using GenericObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace StrategyPattern
{
    public class AudioCommands: RoomCommandBase
    {
        public Delegates.RoomCommandDelegate StartAudio;
        public Delegates.RoomCommandDelegate StopAudio;
        public Delegates.RoomCommandDelegate PauseAudio;
        public Delegates.RoomCommandDelegate ResumeAudio;

        public override void BindCommands()
        {
            _commands = new Dictionary<GenericEnums.SignalType, Delegates.RoomCommandDelegate>();
            _commands.Add(GenericEnums.SignalType.Start, StartAudio);
            _commands.Add(GenericEnums.SignalType.Stop, StopAudio);
            _commands.Add(GenericEnums.SignalType.Pause, PauseAudio);
            _commands.Add(GenericEnums.SignalType.Resume, ResumeAudio);
        }
    }
}
