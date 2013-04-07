using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericObjects;
using Utils;

namespace StrategyPattern
{
    public class RoomCommandInvoker : IRoomCommandInvoker
    {
        Dictionary<GenericEnums.RoomType, IRoomCommands> commands;

        public RoomCommandInvoker(ControllerRoomHandlers roomHandlers)
        {
            // provide the video/audio/remoting event handlers from the controller

            VideoCommands videoCommands = new VideoCommands()
            {
                StartVideo = roomHandlers.Video[GenericEnums.SignalType.Start],
                StopVide = roomHandlers.Video[GenericEnums.SignalType.Stop],
                PauseVideo = roomHandlers.Video[GenericEnums.SignalType.Pause],
                ResumeVideo = roomHandlers.Video[GenericEnums.SignalType.Resume]
            };
            videoCommands.BindCommands();
      
            TransferCommands transferCommands = new TransferCommands()
            {
                SendFile = roomHandlers.Transfer[GenericEnums.SignalType.Start]
            };
            transferCommands.BindCommands();

            RemotingCommands remotingCommands = new RemotingCommands()
            {
                StartRemoting = roomHandlers.Remoting[GenericEnums.SignalType.Start],
                StopRemoting = roomHandlers.Remoting[GenericEnums.SignalType.Stop],
                PauseRemoting = roomHandlers.Remoting[GenericEnums.SignalType.Pause],
                ResumeRemoting = roomHandlers.Remoting[GenericEnums.SignalType.Resume]
            };
            remotingCommands.BindCommands();

            AudioCommands audioCommands = new AudioCommands()
            {
                StartAudio = roomHandlers.Audio[GenericEnums.SignalType.Start],
                StopAudio = roomHandlers.Audio[GenericEnums.SignalType.Stop],
                PauseAudio = roomHandlers.Audio[GenericEnums.SignalType.Pause],
                ResumeAudio = roomHandlers.Audio[GenericEnums.SignalType.Resume]
            };
            audioCommands.BindCommands();

            commands = new Dictionary<GenericEnums.RoomType, IRoomCommands>();
            commands.Add(GenericEnums.RoomType.Audio, audioCommands);
            commands.Add(GenericEnums.RoomType.Video, videoCommands);
            commands.Add(GenericEnums.RoomType.Send, transferCommands);
            commands.Add(GenericEnums.RoomType.Remoting, remotingCommands);
        }

        public void PerformCommand(object sender, RoomActionEventArgs args)
        {
            commands[args.RoomType].Execute(sender, args);
        }
    }
}
