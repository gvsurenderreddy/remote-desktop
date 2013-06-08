
using AudioStreaming;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using Utils;

namespace GenericObjects
{
    public class AudioStreamManager : IAudioStreamManager
    {
        #region private members

        //ManualResetEvent _syncAudioInstance = new ManualResetEvent(false);
        AudioStream _audioStream;
        EventHandler _onCaptureAvailable;
        int _timerInterval;
        ManualResetEvent _syncCaptures = new ManualResetEvent(true);

        Dictionary<string, object> _syncPartnerCaptures = new Dictionary<string, object>();

        Dictionary<string, AudioQueue> _pendingCapturesQueue = new Dictionary<string, AudioQueue>();
        Dictionary<string, DateTime> _activeCapturesQueue = new Dictionary<string, DateTime>();

        #endregion

        #region c-tor

        public AudioStreamManager(int timerInterval, EventHandler onCaptureAvailable)
        {
            _timerInterval = timerInterval;
            _onCaptureAvailable = onCaptureAvailable;
        }

        #endregion

        #region private methods

        void PlayCapture(byte[] capture, string senderIdentity, double captureLengthInSeconds)
        {
            try
            {
                if (capture == null || capture.Length == 0)
                {
                    return;
                }
                
                bool eliminateNoise = bool.Parse(ConfigurationManager.AppSettings["eliminateNoise"]);
                if (eliminateNoise)
                {
                    NoiseEliminator eliminator = new NoiseEliminator(capture);
                    byte[] clear = eliminator.EliminateNoise();
                    if (clear != null && clear.Length > 0)
                    {
                        PlaySound(clear, senderIdentity, captureLengthInSeconds);
                    }
                    else
                    {
                        PlaySound(capture, senderIdentity, captureLengthInSeconds);
                    }
                }
                else
                {
                    PlaySound(capture, senderIdentity, captureLengthInSeconds);
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
            finally
            {
                // todo: remove this log
                //Tools.Instance.Logger.LogInfo("play capture exit");
                GC.Collect();
            }
        }

        void PlaySound(byte[] capture, string senderIdentity, double captureLengthInSeconds)
        {
            try
            { 
                AudioCapture toPlay = null;
                //object toLock = new object();
                //if (_syncPartnerCaptures.ContainsKey(senderIdentity))
                //{
                //    toLock = _syncPartnerCaptures[senderIdentity];
                //}
                //else
                //{
                //    _syncPartnerCaptures.Add(senderIdentity, toLock);
                //}

                //lock (toLock)
                //{
               
                //    bool mustWait = false;
                //    // wait for the previos captures sent by the same partner to finish playing

                //    if (_activeCapturesQueue != null && _activeCapturesQueue.ContainsKey(senderIdentity))
                //    {

                //        // if there is any currently playing capture, then the latest received capture should wait before playing
                //        mustWait = true;
                //    }
                //    if (mustWait)
                //    {
                //        // add the received capture to the partner's pending queue
                //        AudioCapture newCapture = new AudioCapture()
                //        {
                //            Capture = capture,
                //            ReceiveTimestamp = DateTime.Now
                //        };
                //        AudioQueue queue = new AudioQueue();
                //        if (!_pendingCapturesQueue.ContainsKey(senderIdentity))
                //        {
                //            _pendingCapturesQueue.Add(senderIdentity, queue);
                //        }
                //        queue = _pendingCapturesQueue[senderIdentity];
                //        queue.AddCapture(newCapture);

                //        // pick the oldest capture that has to be played
                //        toPlay = queue.PopCapture();

                //        // derminte how much time you must wait for the active capture to finish playing
                //        double elapsedSeconds = DateTime.Now.Subtract(_activeCapturesQueue[senderIdentity]).TotalSeconds;
                //        if (elapsedSeconds < captureLengthInSeconds)
                //        {
                //            TimeSpan toWait = TimeSpan.FromSeconds(elapsedSeconds);
                //            Thread.Sleep(toWait);
                //        }
                //    }
                //    else
                    {
                        toPlay = new AudioCapture()
                        {
                            Capture = capture
                        };
                    }
                    if (toPlay != null && toPlay.Capture != null && toPlay.Capture.Length > 0)
                    {
                        if (_activeCapturesQueue.ContainsKey(senderIdentity))
                        {
                            _activeCapturesQueue[senderIdentity] = DateTime.Now;
                        }
                        else
                        {
                            _activeCapturesQueue.Add(senderIdentity, DateTime.Now);
                        }

                        SoundEffect sound = new SoundEffect(toPlay.Capture, Microphone.Default.SampleRate, AudioChannels.Mono);
                        SoundEffect.MasterVolume = 1f;
                        sound.Play();

                        // decide if to remove or not this sleep
                        TimeSpan ts = TimeSpan.FromMilliseconds(captureLengthInSeconds * 1000);
                        Thread.Sleep(ts);

                        //todo: remove this log
                        //Tools.Instance.Logger.LogInfo("played capture of " + capture.Length + " bytes");

                        sound.Dispose();
                    }
                    else
                    {
                        // todo: remove this log
                        //Tools.Instance.Logger.LogInfo("nothing to play");
                    }
                //}
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        void OnAudioReady(object sender, EventArgs e)
        {
            try
            {
                _syncCaptures.WaitOne(); // used to wait until the room button action is processed
                //_syncAudioInstance.WaitOne();

                AudioCaptureEventArgs eventArgs = (AudioCaptureEventArgs)e;
                byte[] capture = eventArgs.Capture;
    
                if (capture != null && capture.Length > 0)
                {
                    _onCaptureAvailable.BeginInvoke(this, eventArgs
                   , null, null);
                }
                else
                {
                    if (_audioStream.IsRunning)
                    {
                        _audioStream.StartAudio();
                    }
                }

            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion

        #region public methods

        public void WaitRoomButtonAction(bool wait)
        {
            try
            {
                if (wait)
                {
                    _syncCaptures.Reset();
                }
                else
                {
                    _syncCaptures.Set();
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void StartStreaming()
        {
            try
            {
                if (_audioStream == null)
                {
                    Thread t = new Thread(delegate()
                    {
                        try
                        {
                            //_syncAudioInstance.Reset();
                            _audioStream = new AudioStream(this.OnAudioReady);
                            //_syncAudioInstance.Set();
                            _audioStream.Run();
                        }
                        catch (Exception ex)
                        {
                            Tools.Instance.Logger.LogError(ex.ToString());
                        }
                    });
                    t.Start();
                }
                else
                {
                    if (_audioStream.IsRunning == false)
                    {
                        _audioStream.StartAudio();
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void StopStreaming()
        {
            try
            {
                _syncCaptures.Reset();
                if (_audioStream != null)
                {
                    _audioStream.StopAudio();
                }
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
            finally
            {
                _syncCaptures.Set();
            }
        }

        public void PlayAudioCapture(byte[] capture, string senderIdentity, double captureLengthInSeconds)
        {
            Thread t = new Thread(delegate()
            {
                PlayCapture(capture, senderIdentity, captureLengthInSeconds);
            });
            t.Start();
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
