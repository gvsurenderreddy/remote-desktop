using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.VisualBasic.Devices;
using System.IO;
using System.Runtime.InteropServices;

namespace AudioCaptureLib
{
    public class AudioCapture
    {
        #region public members

        public delegate void AudioEventHandler(object source, AudioEventArgs e);
        event AudioEventHandler _soundCaptured;

        #endregion

        #region private members

        bool _audioStreamRunning = true;
        bool _audioCaptureReady = false;

        #endregion

        #region c-tor

        public AudioCapture(AudioCapture.AudioEventHandler handler)
        {
            _soundCaptured += handler;
        }

        #endregion

        #region private methdos

        public void StartAudioStreaming()
        {
            _audioStreamRunning = true;
            Thread t = new Thread(
            delegate()
            {
                StartRecording();

                while (_audioStreamRunning)
                {
                    TimeSpan ts = new TimeSpan(0, 0, 0, 3, 0);
                    Thread.Sleep(ts);
                    SaveRecorded();
                    if (File.Exists("c:\\record.wav") && new FileInfo("c:\\record.wav").Length > 10 * 1024)
                    {
                        long length3 = new FileInfo("c:\\record.wav").Length;

                        _audioCaptureReady = true;
                        SendCapturedSound();
                        _audioCaptureReady = false;
                    }

                    long length2 = new FileInfo("c:\\record.wav").Length;

                    StopRecording();
                    _audioStreamRunning = true;
                    StartRecording();
                }
            });
            t.Start();
        }

        void SendCapturedSound()
        {
            if (File.Exists("c:\\record.wav"))
            {
                byte[] bytes = File.ReadAllBytes(@"c:\record.wav");
                _soundCaptured(this, new AudioEventArgs(bytes));
            }
        }

        void StartRecording()
        {
            // record from microphone
            Win32APIMethods.mciSendString("open new Type waveaudio Alias recsound", "", 0, 0);
            Win32APIMethods.mciSendString("record recsound", "", 0, 0);
        }

        public void StopRecording()
        {
            Win32APIMethods.mciSendString("close recsound ", "", 0, 0);
            Computer c = new Computer();
            c.Audio.Stop();
            File.Delete("c:\\record.wav");
            _audioStreamRunning = false;
        }

        void SaveRecorded()
        {
            // save
            Win32APIMethods.mciSendString("save recsound c:\\record.wav", "", 0, 0);
        }

        #endregion

        #region proprieties

        public bool AudioCaptureReady
        {
            get 
            { 
                return _audioCaptureReady; 
            }
        }

        #endregion
    }
}
