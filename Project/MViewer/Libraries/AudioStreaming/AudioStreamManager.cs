using MicrophoneSample;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

namespace AudioStreaming
{
    public class AudioStreamManager : IAudioStreamManager
    {
        #region private members

        System.Timers.Timer _timer;
        AudioStream _audioStream;
        EventHandler _onCaptureAvailable;

        #endregion

        #region c-tor

        public AudioStreamManager(EventHandler onCaptureAvailable)
        {
            _onCaptureAvailable = onCaptureAvailable;
            _audioStream = new AudioStream();
        }

        #endregion

        #region private methods

        void OnAudioReady(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();

            _audioStream.SyncChunk.Reset();

            byte[] file = _audioStream.Stream.GetBuffer();
            _onCaptureAvailable.Invoke(this, 
            _audioStream.Stream = new MemoryStream();

            _audioStream.SyncChunk.Set();

            _timer.Start();
        }

        #endregion

        #region public methods

        public void StartStreaming()
        {
            _timer = new System.Timers.Timer(3 * 1000);
            _timer.Elapsed += new ElapsedEventHandler(OnAudioReady);

            Thread t = new Thread(delegate()
            {
                _audioStream = new AudioStream();
                _audioStream.Run();
            });
            t.Start();

            _timer.Start();
        }

        public void StopStreaming()
        {
            _timer.Stop();

            _audioStream.StopAudio();

            _audioStream.Exit();
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
    }
}
