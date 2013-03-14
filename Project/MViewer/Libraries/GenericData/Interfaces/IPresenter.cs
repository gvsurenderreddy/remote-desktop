using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDataLayer
{
    public interface IPresenter
    {
        void StartVideoPresentation();
        void StopVideoPresentation();

        void StartAudioPresentation();
        void StopAudioPresentation();

        void StartRemotingPresentation();
        void StopRemotingPresentation();

        bool RemotingCaptureClosed();
    }
}
