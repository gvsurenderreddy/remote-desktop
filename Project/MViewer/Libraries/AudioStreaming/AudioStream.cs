using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO.IsolatedStorage;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace AudioStreaming
{
    public class AudioStream : Microsoft.Xna.Framework.Game
    {
        #region private members

        GraphicsDeviceManager _graphicsManager;
        ManualResetEvent _syncChunk = new ManualResetEvent(true);
        ManualResetEvent _syncStatus = new ManualResetEvent(true);
        byte[] _buffer;
        MemoryStream _stream;
        private Microphone _microphone;
        bool _isRunning;

        #endregion

        #region proprieties

        public bool IsRunning
        {
            get { return _isRunning; }
        }

        public MemoryStream Stream
        {
            get { return _stream; }
            set { _stream = value; }
        }

        public ManualResetEvent SyncChunk
        {
            get
            {
                return _syncChunk;
            }
            set
            {
                _syncChunk = value;
            }
        }

        #endregion

        #region c-tor

        public AudioStream()
        {
            try
            {
                _microphone = Microphone.Default;

                _graphicsManager = new GraphicsDeviceManager(this);
                _graphicsManager.PreferredBackBufferHeight = 1;
                _graphicsManager.PreferredBackBufferWidth = 1;

                Form gameWindowForm = (Form)Form.FromHandle(this.Window.Handle);
                gameWindowForm.Hide();
                gameWindowForm.ShowInTaskbar = false;
                gameWindowForm.Opacity = 0;

                FrameworkDispatcher.Update();
                _microphone.BufferDuration = TimeSpan.FromSeconds(1);
                _buffer = new byte[_microphone.GetSampleSizeInBytes(_microphone.BufferDuration)];
                _microphone.BufferReady += OnBufferReady;
                _isRunning = true;
                _stream = new MemoryStream();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot initialize the audio module. Please restart the app.",
                    "Audio Failure", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        #endregion

        #region private methods

        private void OnBufferReady(object sender, EventArgs e)
        {
            SyncChunk.WaitOne();

            _microphone.GetData(_buffer);
            if (_stream == null)
            {
                _stream = new MemoryStream();
            }
            _stream.Write(_buffer, 0, _buffer.Length);
        }

        #endregion

        #region override methods

        protected override void Draw(GameTime gameTime)
        {
            try
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                
                base.Draw(gameTime);
            }
            catch { }
        }

        protected override void Initialize()
        {
            base.Initialize();
            _stream = new MemoryStream();
            _microphone.Start();
            _isRunning = true;
        }

        protected override void Update(GameTime gameTime)
        {
            _syncStatus.WaitOne();
            if (!_isRunning)
            {
                _microphone.Stop();
                _microphone = null;
            }

            base.Update(gameTime);
        }

        #endregion

        #region public methods

        public void StopAudio()
        {
            _syncStatus.Reset();
            _isRunning = false;
            _syncStatus.Set();
        }

        #endregion
    }
}   