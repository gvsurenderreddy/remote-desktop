using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AudioStreaming
{
    public class NoiseEliminator
    {
        byte[] _capture;

        public NoiseEliminator(byte[] inputCapture)
        {
            _capture = inputCapture;
        }

        public byte[] EliminateNoise()
        {
            Noise();
            FileStream fs = new FileStream("noisy.wav", FileMode.Open);
            byte[] clear = new byte[fs.Length];
            fs.Read(clear, 0, clear.Length);
            fs.Close();
            fs.Dispose();

            return clear;
        }

        void Noise()
        {
            FileStream fs = new FileStream("noisy.wav", FileMode.CreateNew);
            fs.Write(_capture, 0, _capture.Length);
            fs.Close();
            fs.Dispose();

            string soxDir = Directory.GetCurrentDirectory() + "\\sox\\";

            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = soxDir + "sox.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "noisy.wav −n trim 0 1 noiseprof | play noisy.wav noisered";

            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch
            {
                // Log error.
            }
        }
    }
}
