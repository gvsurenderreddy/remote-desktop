using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericDataLayer;
using Utils;

namespace GenericDataLayer
{
    public class VideoCommands : RoomCommandBase
    {
        public Delegates.RoomCommandDelegate StartVideoChat;
        public Delegates.RoomCommandDelegate StopVideChat;
        public Delegates.RoomCommandDelegate PauseVideoChat;
        public Delegates.RoomCommandDelegate ResumeVideoChat;


        // todo: move these to audio class
        public Delegates.RoomCommandDelegate StartAudioChat;
        public Delegates.RoomCommandDelegate StopAudioChat;


        public override void BindCommands()
        {
            _commands = new Dictionary<GenericEnums.SignalType, Delegates.RoomCommandDelegate>();
            _commands.Add(GenericEnums.SignalType.Start, StartVideoChat);
            _commands.Add(GenericEnums.SignalType.Stop, StopVideChat);
            _commands.Add(GenericEnums.SignalType.Pause, PauseVideoChat);
            _commands.Add(GenericEnums.SignalType.Resume, ResumeVideoChat);
        }
    }
}
