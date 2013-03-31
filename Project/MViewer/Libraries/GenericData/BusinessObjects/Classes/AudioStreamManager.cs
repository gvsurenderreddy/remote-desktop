
using AudioStreaming;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

namespace GenericObjects
{
    public class AudioStreamManager : IAudioStreamManager
    {
        #region private members

        ManualResetEvent _syncAudioInstance = new ManualResetEvent(false);
        System.Timers.Timer _timer;
        AudioStream _audioStream;
        EventHandler _onCaptureAvailable;

        #endregion

        #region c-tor

        public AudioStreamManager(EventHandler onCaptureAvailable)
        {
            _onCaptureAvailable = onCaptureAvailable;
        }

        #endregion

        #region private methods

        void OnAudioReady(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();

            _syncAudioInstance.WaitOne();
            Thread.Sleep(200);

            //todo: fix the issue that is preventing the audioStream object from being instanced before being used below
            _audioStream.SyncChunk.Reset();

            byte[] file = _audioStream.Stream.GetBuffer();
            //_onCaptureAvailable.Invoke(this, AudioEventArgs
            _audioStream.Stream = new MemoryStream();

            if (file != null && file.Length > 0)
            {
                _onCaptureAvailable.Invoke(this, new AudioCaptureEventArgs()
                {
                    Capture = file
                });
            }
            _audioStream.SyncChunk.Set();

            _timer.Start();
        }

        #endregion

        #region public methods

        public void StartStreaming()
        {
            _timer = new System.Timers.Timer(1 * 1000);
            _timer.Elapsed += new ElapsedEventHandler(OnAudioReady);

            Thread t = new Thread(delegate()
            {
                _syncAudioInstance.Reset();

                _audioStream = new AudioStream();

                _syncAudioInstance.Set();

                _audioStream.Run();
            });
            t.Start();

            _timer.Start();
        }

        public void StopStreaming()
        {
            if (_timer != null)
            {
                _timer.Stop();
            }
            if (_audioStream != null)
            {
                _audioStream.StopAudio();

                _audioStream.Exit();
            }
        }

        public void PlayAudioCapture(byte[] capture)
        {
            if (capture == null || capture.Length == 0)
            {
                return;
            }
            var sound = new SoundEffect(capture, Microphone.Default.SampleRate, AudioChannels.Mono);
            SoundEffect.MasterVolume = 1f;
            sound.Play();
        }

        #endregion

        public bool AudioCaptureClosed
        {
            get
            {
                return _audioStream == null? true : _audioStream.IsRunning == false;
            }
        }
    }
}
